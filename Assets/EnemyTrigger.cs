using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class EnemyTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log("OnTriggerEnter2D");
            // var player = collision.gameObject.GetComponent<PlayerController>();
            if (collider.gameObject.tag == "Player")
            {
                var ev = Schedule<PlayerEnemyCollision>();
                ev.player = collider.gameObject.GetComponent<PlayerController>();
                ev.enemy = gameObject.transform.parent.GetComponent<EnemyController>();
            }
        }
    }
}

