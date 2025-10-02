using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestController : MonoBehaviour
{
    public float Speed = 400f;
    public PlayerInput InputModule;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InputAction upAction = InputModule.actions.FindAction("Up");
        InputAction downAction = InputModule.actions.FindAction("Down");
        InputAction leftAction = InputModule.actions.FindAction("Left");
        InputAction rightAction = InputModule.actions.FindAction("Right");

        float velo = Speed * Time.deltaTime;

        gameObject.transform.position += new Vector3(
            -velo * (leftAction.IsPressed() ? 1 : 0) + velo * (rightAction.IsPressed() ? 1 : 0),
            -velo * (downAction.IsPressed() ? 1 : 0) + velo * (upAction.IsPressed() ? 1 : 0),
            0f
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered, but we're in the player controller");
    }
}
