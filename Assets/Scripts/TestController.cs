using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestController : MonoBehaviour
{
    public float Speed = 400f;
    public AudioClip[] m_SfxWhiffs;

    private PlayerInput m_PlayerInput;

    private Animator m_Animator;
    private AudioSource m_AudioSource;

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

        m_Animator.SetBool("ForwardOrBack", leftAction.IsPressed() || rightAction.IsPressed());

        InputAction lightAction = m_PlayerInput.actions.FindAction("Light");
        InputAction mediumAction = m_PlayerInput.actions.FindAction("Medium");
        InputAction heavyAction = m_PlayerInput.actions.FindAction("Heavy");
        InputAction specialAction = m_PlayerInput.actions.FindAction("Special");

        var now = Time.time;

        if ((now - m_lastAttackTime) >= ATTACK_COOLDOWN_TIME)
        {
            if (lightAction.WasPressedThisFrame() && TryAnimTrigger("LightPressed"))
            {
                m_Animator.SetBool("JabToggle", !m_Animator.GetBool("JabToggle"));
                m_lastAttackTime = now;
                PlayRandomizedWhiffEffect();
            }
            else if (mediumAction.WasPressedThisFrame() && TryAnimTrigger("MediumPressed"))
            {
                m_lastAttackTime = now;
                PlayRandomizedWhiffEffect();
            }
            else if (heavyAction.WasPressedThisFrame() && TryAnimTrigger("HeavyPressed"))
            {
                m_lastAttackTime = now;
                PlayRandomizedWhiffEffect();
            }
            else if (specialAction.WasPressedThisFrame() && TryAnimTrigger("SpecialPressed"))
            {
                m_lastAttackTime = now;
                PlayRandomizedWhiffEffect();
            }
        }
    }

    private void PlayRandomizedWhiffEffect()
    {
        AudioClip selectedClip = m_SfxWhiffs[Random.Range(0, m_SfxWhiffs.Length)];

        m_AudioSource.pitch = Random.Range(SFX_PITCH_MOD_MIN, SFX_PITCH_MOD_MAX);
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
}
