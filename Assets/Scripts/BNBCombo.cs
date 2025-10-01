using UnityEngine;
using System;
using System.Collections.Generic;
using static BNBInput;

[System.Serializable]
public class BNBCombo : System.Object
{
    [SerializeField] public List<BNBInputType> Inputs = new List<BNBInputType> { };
    [SerializeField] public string ComboName;
    [SerializeField] public float TimingWindow; // ms

    public BNBCombo(List<BNBInputType> _inputs, string _name, float timingWindow) : base()
    {
        Inputs = _inputs; Inputs.Reverse(); // inputs are reversed relative to declaration order to make searching the queue easier
        ComboName = _name;
        TimingWindow = timingWindow;
    }

    public int Length() { return Inputs.Count; }
}