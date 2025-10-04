using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class KitchenComboInteractable : MonoBehaviour
{
    public int ComboIndex;
    public string CurrentComboName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        ComboIndex = 0;
        SetChildrenVisible(false);
        UpdateCurrentComboName();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered");

        SetChildVisible(ComboIndex, true);
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

    private void SetChildVisible(int childIndex, bool visible)
    {
        transform.GetChild(childIndex).gameObject.SetActive(visible);
    }

    public virtual void AdvanceCombo()
    {
        Debug.Log("Inside KitchenComboInteractable.AdvanceCombo");
        ComboIndex = (ComboIndex + 1) % transform.childCount;
        UpdateCurrentComboName();
        SetChildrenVisible(false);
        SetChildVisible(ComboIndex, true);

        // Call any scripts that want to do something upon combo advance
        SendMessage("OnComboAdvanced");
    }

    private void UpdateCurrentComboName()
    {
        CurrentComboName = transform.GetChild(ComboIndex).GetComponent<ComboIndicatorCanvas>().ComboName;
    }
}
