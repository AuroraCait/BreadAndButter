using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestController : MonoBehaviour
{
    public float Speed = 400f;
    private PlayerInput m_PlayerInput;

    private Animator m_Animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_PlayerInput = GetComponent<PlayerInput>();

        m_Animator.SetBool("ForwardOrBack", false);
        m_Animator.SetBool("JabToggle", false);
    }

    // Update is called once per frame
    void Update()
    {
        InputAction upAction = m_PlayerInput.actions.FindAction("Up");
        InputAction downAction = m_PlayerInput.actions.FindAction("Down");
        InputAction leftAction = m_PlayerInput.actions.FindAction("Left");
        InputAction rightAction = m_PlayerInput.actions.FindAction("Right");

        float velo = Speed * Time.deltaTime;

        gameObject.transform.position += new Vector3(
            -velo * (leftAction.IsPressed() ? 1 : 0) + velo * (rightAction.IsPressed() ? 1 : 0),
            -velo * (downAction.IsPressed() ? 1 : 0) + velo * (upAction.IsPressed() ? 1 : 0),
            0f
        );

        m_Animator.SetBool("ForwardOrBack", leftAction.IsPressed() || rightAction.IsPressed());
    }
}
