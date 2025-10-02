using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariable<T> : ScriptableObject
{
    [Tooltip("The current value of this variable at runtime.")]
    [SerializeField] protected T runtimeValue;

    public virtual T Value
    {
        get => runtimeValue;
        set => runtimeValue = value;
    }
}