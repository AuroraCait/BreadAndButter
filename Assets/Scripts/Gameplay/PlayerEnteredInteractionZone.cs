using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when a player enters a trigger with a InteractionZone component.
    /// </summary>
    /// <typeparam name="PlayerEnteredInteractionZone"></typeparam>
    public class PlayerEnteredInteractionZone : Simulation.Event<PlayerEnteredInteractionZone>
    {
        public InteractionZone interactionzone;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            Simulation.Schedule<PlayerInteractable>(0);
        }
    }
}
