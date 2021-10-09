using Platformer.Core;
using Platformer.Mechanics;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the Jump Input is deactivated by the user, cancelling the upward velocity of the jump.
    /// </summary>
    /// <typeparam name="PlayerStopJump"></typeparam>
    public class PlayerStopJump : Simulation.Event<PlayerStopJump>
    {
        public PlayerController player;

        public override void Execute()
        {
            Debug.Log("PlayerStopJump");
        }
    }
}
