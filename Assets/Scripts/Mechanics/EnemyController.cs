using System.Collections;
using System.Collections.Generic;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour
    {
        public PatrolPath path;
        public AudioClip ouch;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        internal Animator animator;
        SpriteRenderer spriteRenderer;

        public Bounds Bounds => _collider.bounds;

        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        // void OnCollisionEnter2D(Collision2D collision)
        // {
        //     var player = collision.gameObject.GetComponent<PlayerController>();
        //     if (player != null)
        //     {
        //         Debug.Log("Player hit");
        //         var ev = Schedule<PlayerEnemyCollision>();
        //         ev.player = player;
        //         ev.enemy = this;
        //     }
        // }

        // void OnTriggerEnter2D(Collider2D collider)
        // {
        //     Debug.Log("OnTriggerEnter2D");
        //     // var player = collision.gameObject.GetComponent<PlayerController>();
        //     if (collider.gameObject.tag == "Player")
        //     {
        //         var ev = Schedule<PlayerEnemyCollision>();
        //         ev.player = collider.gameObject.GetComponent<PlayerController>();
        //         ev.enemy = this;
        //     }
        // }

        void Update()
        {
            if (path != null)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
            }
        }

    }
}
