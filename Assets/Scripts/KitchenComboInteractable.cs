using UnityEngine;

public class KitchenComboInteractable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // foreach (var r in GetComponentsInChildren<Renderer>())
        // {
        //     r.enabled = false;
        // }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        Debug.Log("Trigger entered");

        // if (collision.gameObject.CompareTag("Player"))
        // {
        //     foreach (var r in GetComponentsInChildren<Renderer>())
        //     {
        //         r.enabled = true;
        //     }
        // }
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        Debug.Log("Trigger exited");
        
        // if (collision.gameObject.CompareTag("Player"))
        // {
        //     foreach (var r in GetComponentsInChildren<Renderer>())
        //     {
        //         r.enabled = false;
        //     }
        // }
    }
}
