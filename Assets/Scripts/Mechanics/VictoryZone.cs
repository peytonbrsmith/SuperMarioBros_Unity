using Platformer.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.Core.Simulation;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marks a trigger as a VictoryZone, usually used to end the current game level.
    /// </summary>
    public class VictoryZone : MonoBehaviour
    {
        public PlayerController player;
        public AudioSource audioSource;
        public AudioClip victorySound;

        void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                // stop audiosource
                player.win();
                audioSource.Stop();
                if (player.audioSource && victorySound)
                    player.audioSource.PlayOneShot(victorySound);
                StartCoroutine("winSound");
                // var ev = Schedule<PlayerEnteredVictoryZone>();
                // ev.victoryZone = this;
            }
        }

        IEnumerator winSound() {
            yield return new WaitForSecondsRealtime(2.5f);
            if (player.audioSource && player.winAudio)
                    player.audioSource.PlayOneShot(player.winAudio);
            yield return new WaitForSecondsRealtime(9.0f);
            SceneManager.LoadScene("Overworld 1-1");
        }
    }
}
