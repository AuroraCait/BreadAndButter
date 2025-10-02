using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// InteractionZone components mark a collider which will schedule a
    /// PlayerEnteredInteractionZone event when the player enters the trigger.
    /// </summary>
    public class InteractionZone : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                var ev = Schedule<PlayerEnteredInteractionZone>();
                ev.interactionzone = this;
            }
        }
    }
}