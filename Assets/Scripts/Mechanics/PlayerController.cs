using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public AudioClip winAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public BoxCollider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        public Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public bool crouch;
        public bool big;

        public Bounds Bounds;

        private GameObject flag;

        public int coinCount = 0;

        public BoxCollider2D crouchCollider;
        public BoxCollider2D standCollider;



        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = crouchCollider;
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            // Physics2D.IgnoreLayerCollision(0, 5, true);
        }

        // private void OnCollisionEnter2D(Collision2D collision)
        // {
        //     Debug.Log("Collision");
        //     var tilemap = collision.gameObject.GetComponent<Tilemap>();
        //     var grid = tilemap.layoutGrid;
        //     var cell = grid.WorldToCell(collision.transform.position);
        //     cell.y += 1;
        //             Debug.Log(collision.GetContact(0).point);
        //     Debug.Log(collision.GetContact(0).normal);
        //     Debug.Log(cell);

        //     if (collision.GetContact(0).normal.y == 1.0f)
        //     {
        //         cell.y += 2;
        //         Debug.Log("Breaking tile at " + cell);
        //         tilemap.SetTile(cell, null);
        //     }
        //     // else if (collision.GetContact(0).normal.y == 1.0f)

        // }

        protected override void Update()
        {
            if (!health.IsAlive)
            {
                move.x = 0.0f;
            }
            UpdateJumpState();
            base.Update();
            if (health.currentHP > 1)
            {
                big = true;
                jumpTakeOffSpeed = 35;
                animator.SetBool("big", true);
            }
            else
            {
                big = false;
                jumpTakeOffSpeed = 30;
                animator.SetBool("big", false);
            }
            if (crouch)
            {
                collider2d = crouchCollider;
            }
            else if (big && !crouch)
            {
                collider2d = standCollider;
            }
            Bounds = collider2d.bounds;
            if (gameObject.tag == "Player")
            {
                var pos = new Vector3(transform.position.x, Bounds.max.y + 0.3f, 0);
                Collider2D collision = Physics2D.OverlapPoint(pos, contactFilter.layerMask);
                if (collision != null && health.currentHP >= 1)
                {
                    if (collision.gameObject.tag == "Powerblock")
                    {
                        collision.gameObject.GetComponent<PowerBlock>().BreakTile(pos);
                    }
                    if (collision.gameObject.tag == "Breakable")
                    {
                        if (big)
                        {
                            collision.gameObject.GetComponent<Bricks>().BreakTile(pos);
                        }
                        else
                        {
                            collision.gameObject.GetComponent<Bricks>().BumpTile();
                        }
                    }
                    if (collision.gameObject.tag == "Coinblock")
                    {
                        collision.gameObject.GetComponent<CoinBlock>().BreakTile(pos);
                    }
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, Bounds.max.y + 0.3f, 0), 0.1f);
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        // Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = model.jumpDeceleration * gravityModifier;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        public void win()
        {
            gravityModifier = 0;
            controlEnabled = false;
            move.x = 0;
            move.y = 0;
            StartCoroutine("winAnim");
        }

        IEnumerator winAnim() {
            flag = GameObject.Find("Flag");
            flag.GetComponent<Flag>().enabled = true;
            gravityModifier = 2.0f;
            yield return new WaitForSecondsRealtime(2.5f);
            move.x = 0.5f;
        }

        public void OnMove(InputValue value)
		{
            crouch = false;
            animator.SetBool("crouch", crouch);
            if (controlEnabled)
			    move.x = value.Get<float>();
		}

        public void OnJump(InputValue value)
		{
            if (controlEnabled)
            {
                if (jumpState == JumpState.Grounded)
                    jumpState = JumpState.PrepareToJump;
                // else
                // {
                //     stopJump = true;
                //     Schedule<PlayerStopJump>().player = this;
                // }
            }
		}

        public void OnCrouch(InputValue value)
		{
            if (controlEnabled && jumpState == JumpState.Grounded && move.x == 0)
            {
                crouch = (value.isPressed);
                if (crouch && big)
                {
                    animator.SetBool("crouch", crouch);
                    standCollider.enabled = false;
                    crouchCollider.enabled = true;
                    collider2d = crouchCollider;
                    Bounds = crouchCollider.bounds;
                }
                else if (big)
                {
                    animator.SetBool("crouch", crouch);
                    standCollider.enabled = true;
                    crouchCollider.enabled = false;
                    collider2d = standCollider;
                    Bounds = standCollider.bounds;
                }
            }
            else if (big)
            {
                crouch = false;
                animator.SetBool("crouch", crouch);
            }
		}

        // public void OnCrouchRelease(InputValue value)
        // {
        //     Debug.Log(value.isPressed);
        // }

        // public void CrouchInput(bool newCrouchState)
		// {
		// 	crouch = newCrouchState;
		// }

        // IEnumerator flag1() {

        // }
    }
}
