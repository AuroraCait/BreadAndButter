using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player can interact with a zone.
    /// </summary>
    /// <typeparam name="PlayerInteractable"></typeparam>
    public class PlayerInteractable : Simulation.Event<PlayerInteractable>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            Debug.LogWarning("Player can interact in this zone");
        }
    }
}