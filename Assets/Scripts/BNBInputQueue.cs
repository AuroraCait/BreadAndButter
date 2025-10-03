using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.InputSystem;
using System;
using System.Linq;
using System.Xml.Schema;
using Unity.VisualScripting;
using static BNBInput;

[System.Serializable]
public class BNBInputQueue : MonoBehaviour
{
    // "Global" combos
    [Tooltip("This is where all combo definitions should go; objects that need to reference a combo should get it from here.")]

    [SerializeField] public List<BNBCombo> AllCombos;

    public BNBInputQueue() : base() { InputQueue = new LinkedList<BNBInput>(); LastDPadState = (false, false, false, false); }

    // I can't imagine we'll implement combos any longer than this.
    private readonly int INPUT_QUEUE_MAX_SIZE = 16;

    // Not going to use callbacks for player input, since we want to control the order in which we enqueue
    // inputs on a given frame
    public void Update()
    {
        // Read actions from the PlayerInput module and add them to the input queue...
        bool inputMade = ReadAndEnqueueActions();

        // ...then determine whether any of those constitute an available combo.
        if (inputMade) { SearchForCombos(); }

        // Do some housekeeping on the input queue.
        CleanupInputQueue();
    }

    private void CleanupInputQueue()
    {
        while (InputQueue.Count > INPUT_QUEUE_MAX_SIZE)
        {
            InputQueue.RemoveLast();
        }
    }

    private bool ReadAndEnqueueActions()
    {
        // TODO this should reference some player object parent to determine the facing direction
        bool facingRight = true;

        bool inputMade = false;

        InputActionAsset frameActions = InputModule.actions;

        // Process directions/movement first.
        InputAction upAction = frameActions.FindAction("Up");
        InputAction downAction = frameActions.FindAction("Down");
        InputAction leftAction = frameActions.FindAction("Left");
        InputAction rightAction = frameActions.FindAction("Right");

        DPadState = (upAction.IsPressed(), leftAction.IsPressed(), rightAction.IsPressed(), downAction.IsPressed());

        // There's probably a more idiomatic way to do this, but oh well.
        // TODO this setup prioritizes upward inputs over downward in cases where both up and down can be pressed
        // simultaneously (e.g. keyboard), this could end up feeling bad
        if (DPadState != LastDPadState)
        {
            inputMade = true;

            if (DPadState.Up)
            {
                if (DPadState.Left)
                {
                    EnqueueInput(facingRight ? BNBInputType.UpBack : BNBInputType.UpForward);
                }
                else if (DPadState.Right)
                {
                    EnqueueInput(facingRight ? BNBInputType.UpForward : BNBInputType.UpBack);
                }
                else
                {
                    EnqueueInput(BNBInputType.Up);
                }
            }
            else if (DPadState.Down)
            {
                if (DPadState.Left)
                {
                    EnqueueInput(facingRight ? BNBInputType.DownBack : BNBInputType.DownForward);
                }
                else if (DPadState.Right)
                {
                    EnqueueInput(facingRight ? BNBInputType.DownForward : BNBInputType.DownBack);
                }
                else
                {
                    EnqueueInput(BNBInputType.Down);
                }
            }
            else
            {
                if (DPadState.Left)
                {
                    EnqueueInput(facingRight ? BNBInputType.Back : BNBInputType.Forward);
                }
                else if (DPadState.Right)
                {
                    EnqueueInput(facingRight ? BNBInputType.Forward : BNBInputType.Back);
                }
            }
        }

        // Then, process "buttons".
        if (frameActions.FindAction("Light").WasPressedThisFrame()) { EnqueueInput(BNBInputType.Light); inputMade = true; }
        if (frameActions.FindAction("Medium").WasPressedThisFrame()) { EnqueueInput(BNBInputType.Medium); inputMade = true; }
        if (frameActions.FindAction("Heavy").WasPressedThisFrame()) { EnqueueInput(BNBInputType.Heavy); inputMade = true; }
        if (frameActions.FindAction("Grab").WasPressedThisFrame()) { EnqueueInput(BNBInputType.Grab); inputMade = true; }
        if (frameActions.FindAction("Special").WasPressedThisFrame()) { EnqueueInput(BNBInputType.Special); inputMade = true; }

        LastDPadState = DPadState;

        return inputMade;
    }

    private void SearchForCombos()
    {
        foreach (var combo in Combos)
        {
            var queueSlice = GetRange(InputQueue, 0, Math.Min(combo.Inputs.Count, InputQueue.Count));

            // Debug.Log("Checking for combo " + combo.Name + "; input queue slice " + ListAsString(queueSlice) + ", combo inputs " + ListAsString(combo.Inputs));

            if ((from input in queueSlice select input.InputType).SequenceEqual(combo.Inputs))
            {
                var timeDelta = Math.Abs(queueSlice.Last().Timestamp - queueSlice.First().Timestamp);

                // The inputs match, but are they within the timing window?
                if (timeDelta <= combo.TimingWindow)
                {
                    Debug.Log("Combo Found - " + combo.ComboName);
                    InputQueue.Clear();
                    break;
                }
                else
                {
                    Debug.Log("Combo found, but missed timing window - " + combo.ComboName + "; input time " + timeDelta + "ms, window " + combo.TimingWindow);
                }
            }
        }
    }

    private void EnqueueInput(BNBInputType inputType)
    {
        var timestamp = Time.time * 1000;

        InputQueue.AddFirst(new BNBInput(
            inputType,
            timestamp
        ));

        if (LogInputs)
        {
            Debug.Log("Input Enqueued - " + inputType + " @ " + timestamp);
        }
    }

    private string ListAsString<T>(ICollection<T> list)
    {
        string res = "{ ";

        foreach (var item in list)
        {
            res += item.ToString() + " ";
        }

        res += "}";

        return res;
    }

    private List<T> GetRange<T>(LinkedList<T> ll, int index, int count)
    {
        List<T> res = new();

        for (int i = 0; i < count; ++i)
        {
            res.Add(ll.ElementAt(index + i));
        }

        return res;
    }

    public BNBCombo GetComboByName(string name)
    {
        return (
            from combo in AllCombos
            where combo.ComboName == name
            select combo
        ).FirstOrDefault();
    }

    public PlayerInput InputModule;
    private LinkedList<BNBInput> InputQueue;
    private readonly BNBCombo[] Combos = {
        new BNBCombo(
            new List<BNBInputType> {BNBInputType.Down, BNBInputType.DownForward, BNBInputType.Forward, BNBInputType.Light},
            "QuarterCircleForwardLight", 500
        ),
        new BNBCombo(
            new List<BNBInputType> {BNBInputType.Down, BNBInputType.DownForward, BNBInputType.Forward, BNBInputType.Medium},
            "QuarterCircleForwardMedium", 500
        ),
        new BNBCombo(
            new List<BNBInputType> {BNBInputType.Down, BNBInputType.DownForward, BNBInputType.Forward, BNBInputType.Heavy},
            "QuarterCircleForwardHeavy", 500
        ),
    };
    private (bool Up, bool Left, bool Right, bool Down) DPadState;
    private (bool Up, bool Left, bool Right, bool Down) LastDPadState;

    private readonly bool LogInputs = true;
}
