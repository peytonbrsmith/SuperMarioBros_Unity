using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            if (player.health.IsAlive)
            {
                // player.health.Die();
                // model.virtualCamera.m_Follow = null;
                // model.virtualCamera.m_LookAt = null;
                // player.collider.enabled = false;
                player.move.x = 0;
                player.move.y = 0;
                player.controlEnabled = false;
                if (player.audioSource && player.respawnAudio)
                    player.audioSource.PlayOneShot(player.respawnAudio);
                player.animator.SetTrigger("hurt");
                player.animator.SetBool("dead", true);
                player.Bounce(30);
                player.standCollider.enabled = false;
                player.crouchCollider.enabled = false;
                // Simulation.Schedule<PlayerSpawn>(4);
                player.StartCoroutine(waitLoad());

            }

            IEnumerator waitLoad()
            {
                yield return new WaitForSeconds(3);
                SceneManager.LoadScene("Overworld 1-1");
            }
        }
    }
}
