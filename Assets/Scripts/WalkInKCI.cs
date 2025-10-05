using UnityEngine;

public class WalkInKCI : MonoBehaviour
{
    private Animator m_Animator;
    private AudioSource m_AudioSource;
    private bool m_closed;

    public AudioClip m_DoorOpenClip;
    public AudioClip m_DoorCloseClip;

    public void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        m_closed = true;
    }

    public void OnComboAdvanced()
    {
        m_Animator.SetTrigger("Toggle");

        m_AudioSource.PlayOneShot(m_closed ? m_DoorOpenClip : m_DoorCloseClip);

        m_closed = !m_closed;
    }
}