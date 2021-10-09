using System;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;
using System.Collections.Generic;
using System.Collections;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Represebts the current vital statistics of some game entity.
    /// </summary>
    public class Health : MonoBehaviour
    {

         public PlayerController player;
         public bool cooldown = false;

        /// <summary>
        /// The maximum hit points for the entity.
        /// </summary>
        public int maxHP = 1;

        /// <summary>
        /// Indicates if the entity should be considered 'alive'.
        /// </summary>
        public bool IsAlive => currentHP > 0;

        public int currentHP;

        /// <summary>
        /// Increment the HP of the entity.
        /// </summary>
        public void Increment()
        {
            currentHP = Mathf.Clamp(currentHP + 1, 0, maxHP);
            player.standCollider.enabled = true;
            player.crouchCollider.enabled = false;
            player.collider2d = player.standCollider;
            player.Bounds = player.standCollider.bounds;
        }

        /// <summary>
        /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
        /// current HP reaches 0.
        /// </summary>
        public void Decrement()
        {
            if (!cooldown)
            {
                currentHP -= 1;
                // StartCoroutine(healthCooldown());
            }
            if (currentHP == 0)
            {
                var ev = Schedule<HealthIsZero>();
                ev.health = this;
            }
            if (currentHP == 1)
            {
                player.big = false;
                player.standCollider.enabled = false;
                player.crouchCollider.enabled = true;
                player.collider2d = player.crouchCollider;
                player.Bounds = player.crouchCollider.bounds;
            }
        }

        /// <summary>
        /// Decrement the HP of the entitiy until HP reaches 0.
        /// </summary>
        public void Die()
        {
            // while (currentHP > 0) Decrement();
        }

        void Awake()
        {
            currentHP = 1;
            player = GetComponent<PlayerController>();
        }

        IEnumerator healthCooldown()
        {
            cooldown = true;
            yield return new WaitForSeconds(3);
            cooldown = false;
        }
    }
}
