using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BNBInput;

public class ComboIndicatorCanvas : MonoBehaviour
{
    [SerializeField] private String ComboName;
    private BNBCombo Combo;
    private int CurrentComboIndex;
    public GameObject InputIconPanel;
    public TMPro.TMP_Text TMPText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Fetch references to the named combos
        var playerObject = GameObject.FindWithTag("Player");
        var inputQueueComponent = playerObject.GetComponent<BNBInputQueue>();

        if (!inputQueueComponent)
        {
            Debug.Log("Unable to get BNBInputQueue component on player GameObject (searching for tag \"Player\")");
        }

        Combo = inputQueueComponent.GetComboByName(ComboName);

        Debug.Log("Creating ComboIndicatorCanvas with " + Combo.Length() + " inputs");

        // TODO need to remove these when advancing combos
        foreach (BNBInputType inputType in Combo.Inputs)
        {
            // WHY THE FUCK DOES THIS NOT WORK
            GameObject iconObj = new();
            Image img = iconObj.AddComponent<Image>();
            iconObj.GetComponent<RectTransform>().SetParent(InputIconPanel.transform);
            img.sprite = InputTypeToSprite(inputType);
            iconObj.SetActive(true);
        }

        TMPText.text = Combo.ComboName;
    }

    private Sprite InputTypeToSprite(BNBInputType inputType) {
        Texture2D tex;
        Debug.Log("Converting input type " + inputType + " to a sprite");
        
        switch (inputType) {
            case BNBInputType.Up:           tex = Resources.Load("Images/InputPrompts/ButtonPrompt_DPadUp") as Texture2D; break;
            case BNBInputType.UpBack:       tex = null; break;
            case BNBInputType.UpForward:    tex = null; break;
            case BNBInputType.Forward:      tex = Resources.Load("Images/InputPrompts/ButtonPrompt_DPadRight") as Texture2D; break;
            case BNBInputType.Back:         tex = Resources.Load("Images/InputPrompts/ButtonPrompt_DPadLeft") as Texture2D; break;
            case BNBInputType.Down:         tex = Resources.Load("Images/InputPrompts/ButtonPrompt_DPadDown") as Texture2D; break;
            case BNBInputType.DownBack:     tex = Resources.Load("Images/InputPrompts/ButtonPrompt_DPadDownBack") as Texture2D; break;
            case BNBInputType.DownForward:  tex = Resources.Load("Images/InputPrompts/ButtonPrompt_DPadDownForward") as Texture2D; break;
            case BNBInputType.Grab:         tex = null; break;
            case BNBInputType.Light:        tex = Resources.Load("Images/InputPrompts/ButtonPrompt_Light") as Texture2D; break;
            case BNBInputType.Medium:       tex = Resources.Load("Images/InputPrompts/ButtonPrompt_Medium") as Texture2D; break;
            case BNBInputType.Heavy:        tex = Resources.Load("Images/InputPrompts/ButtonPrompt_Heavy") as Texture2D; break;
            case BNBInputType.Special:      tex = Resources.Load("Images/InputPrompts/ButtonPrompt_Special") as Texture2D; break;
            default: tex = null; break;
        }

        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
