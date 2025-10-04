using UnityEngine;

public class WalkInKCI : MonoBehaviour
{
    private Animator m_Animator;

    public void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void OnComboAdvanced()
    {
        m_Animator.SetTrigger("Toggle");
    }
}