using UnityEngine;
using System;
using static BNBInput;

[System.Serializable]
public class BNBInput: System.Object
{
    // Enumeration of all possible inputs we can read from the queue
    public enum BNBInputType
    {
        Up,
        UpBack,
        UpForward,
        Forward,
        Back,
        Down,
        DownBack,
        DownForward,
        Grab,
        Light,
        Medium,
        Heavy,
        Special
    }

    public BNBInput(BNBInputType inputType, float timestamp) : base()
    {
        InputType = inputType;
        Timestamp = timestamp;
    }

    public BNBInputType InputType;
    public float Timestamp;
}