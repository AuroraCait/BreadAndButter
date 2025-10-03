using System;
using UnityEngine;

public class KitchenComboInteractable : MonoBehaviour
{
    public int ComboIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ComboIndex = 0;
        SetChildrenVisible(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered");

        SetChildVisible(true, ComboIndex);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exited");

        SetChildrenVisible(false);
    }

    private void SetChildrenVisible(bool visible)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(visible);
        }
    }

    private void SetChildVisible(bool visible, int childIndex)
    {
        transform.GetChild(childIndex).gameObject.SetActive(visible);
    }
}
