using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerInput))]
public class TestController : MonoBehaviour
{
    public float Speed = 400f;
    public AudioClip[] m_SfxWhiffs;
    public AudioClip[] m_SfxCombo;
    public AudioClip[] m_SfxHit;

    // ======================================================

    private PlayerInput m_PlayerInput;

    private Animator m_Animator;
    private AudioSource m_AudioSource;
    private SpriteRenderer m_SpriteRenderer;
    private BNBInputQueue m_InputQueue;
    private BoxCollider2D m_BoxCollider;

    // COMPONENTS

    private float m_lastAttackTime;     // s
    private const float ATTACK_COOLDOWN_TIME = 0.050f; // s

    private const float SFX_PITCH_MOD_MIN = 0.75f;
    private const float SFX_PITCH_MOD_MAX = 1.25f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_PlayerInput = GetComponent<PlayerInput>();
        m_AudioSource = GetComponent<AudioSource>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_InputQueue = GetComponent<BNBInputQueue>();
        m_BoxCollider = GetComponent<BoxCollider2D>();

        m_Animator.SetBool("ForwardOrBack", false);
        m_Animator.SetBool("JabToggle", false);
    }

    // Update is called once per frame
    void Update()
    {
        // InputAction upAction = m_PlayerInput.actions.FindAction("Up");
        // InputAction downAction = m_PlayerInput.actions.FindAction("Down");
        InputAction leftAction = m_PlayerInput.actions.FindAction("Left");
        InputAction rightAction = m_PlayerInput.actions.FindAction("Right");

        float velo = Speed * Time.deltaTime;

        gameObject.transform.position += new Vector3(
            -velo * (leftAction.IsPressed() ? 1 : 0) + velo * (rightAction.IsPressed() ? 1 : 0),
            0f, 0f
        );

        if (rightAction.WasPressedThisFrame())
        {
            m_SpriteRenderer.flipX = false;
        }
        else if (leftAction.WasPressedThisFrame())
        {
            m_SpriteRenderer.flipX = true;
        }

        m_Animator.SetBool("ForwardOrBack", leftAction.IsPressed() || rightAction.IsPressed());

        InputAction lightAction = m_PlayerInput.actions.FindAction("Light");
        InputAction mediumAction = m_PlayerInput.actions.FindAction("Medium");
        InputAction heavyAction = m_PlayerInput.actions.FindAction("Heavy");
        InputAction specialAction = m_PlayerInput.actions.FindAction("Special");

        var now = Time.time;

        if ((now - m_lastAttackTime) >= ATTACK_COOLDOWN_TIME)
        {
            // Cool, we can attack! Are we hitting anything?
            var punchCast = PunchCast();

            bool hit = punchCast && punchCast.transform.gameObject.CompareTag("KitchenInteractable");

            // TODO these should probably check for collision with a BoxCast or similar
            if (lightAction.WasPressedThisFrame() && TryAnimTrigger("LightPressed"))
            {
                m_Animator.SetBool("JabToggle", !m_Animator.GetBool("JabToggle"));
                m_lastAttackTime = now;
                PlayRandomizedAttackEffect(hit);
            }
            else if (mediumAction.WasPressedThisFrame() && TryAnimTrigger("MediumPressed"))
            {
                m_lastAttackTime = now;
                PlayRandomizedAttackEffect(hit);
            }
            else if (heavyAction.WasPressedThisFrame() && TryAnimTrigger("HeavyPressed"))
            {
                m_lastAttackTime = now;
                PlayRandomizedAttackEffect(hit);
            }
            else if (specialAction.WasPressedThisFrame() && TryAnimTrigger("Tatsu"))
            {
                m_lastAttackTime = now;
                // PlayRandomizedWhiffEffect();
                PlayRandomizedAttackEffect(hit);
                PlayRandomizedComboEffect();
            }
        }
    }

    private void PlayRandomizedAttackEffect(bool hitSomething)
    {
        if (hitSomething)
        {
            PlayRandomizedHitEffect();
        }
        else
        {
            PlayRandomizedWhiffEffect();
        }
    }

    private void PlayRandomizedWhiffEffect()
    {
        AudioClip selectedClip = m_SfxWhiffs[Random.Range(0, m_SfxWhiffs.Length)];

        m_AudioSource.pitch = Random.Range(SFX_PITCH_MOD_MIN, SFX_PITCH_MOD_MAX);
        m_AudioSource.PlayOneShot(selectedClip);
    }

    private void PlayRandomizedHitEffect()
    {
        AudioClip selectedClip = m_SfxHit[Random.Range(0, m_SfxHit.Length)];

        m_AudioSource.pitch = Random.Range(SFX_PITCH_MOD_MIN, SFX_PITCH_MOD_MAX);
        m_AudioSource.PlayOneShot(selectedClip);
    }

    private void PlayRandomizedComboEffect()
    {
        AudioClip selectedClip = m_SfxCombo[Random.Range(0, m_SfxCombo.Length)];

        // Let's not pitch bend these.
        m_AudioSource.pitch = 1;
        m_AudioSource.PlayOneShot(selectedClip);
    }

    private bool TryAnimTrigger(string triggerName)
    {
        var currentAnimationState = m_Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimationState.IsName("Idle") || currentAnimationState.IsName("Walk"))
        {
            Debug.Log("Animator in (Idle | Walk) state, sending animation trigger " + triggerName);
            m_Animator.SetTrigger(triggerName);
            return true;
        }
        else
        {
            Debug.Log("Animator not currently in (Idle | Walk)");
            return false;
        }
    }

    // Generalized 2D BoxCast operation
    private RaycastHit2D PunchCast()
    {
        Vector2 playerPos = m_BoxCollider.bounds.center;
        Vector2 castSize = m_BoxCollider.bounds.extents;

        // I have no clue whether this will work.
        return Physics2D.BoxCast(
            playerPos, castSize, 0, Vector2.right,   // TODO this needs to consider facing direction
            m_BoxCollider.bounds.size.x, LayerMask.GetMask("KitchenInteractableLayer")
        );
    }

    private void ComboPerformed(BNBCombo comboPerformed)
    {
        Debug.Log("Got message from BNBInputQueue; combo performed: " + comboPerformed.ComboName);

        // Do another box cast to see if we're hitting an interactable with this combo name.
        RaycastHit2D punchCastInfo = PunchCast();
        GameObject punchCastTarget = punchCastInfo.collider?.gameObject;

        if (punchCastTarget && punchCastTarget.CompareTag("KitchenInteractable"))
        {
            var comboName = punchCastTarget.GetComponent<KitchenComboInteractable>().CurrentComboName;

            if (comboPerformed.ComboName == comboName)
            {
                // Bingo. Play a combo effect and advance that object's combo index.
                PlayRandomizedComboEffect();
                punchCastTarget.GetComponent<KitchenComboInteractable>().AdvanceCombo();
            }
        }
    }
}
