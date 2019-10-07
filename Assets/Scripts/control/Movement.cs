using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace control {
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class Movement : MonoBehaviour {
        public float speed;
        public float jump;
        [SceneObjectsOnly] [Required] public GameObject rayOrigin, bottomLeft, bottomRight;
        public float rayCheckDistance;
        public LayerMask rayLayerMask;
        public LayerMask rayLayerMaskDead;
        private Rigidbody2D rb;
        private bool facingRight = true;
        private Animator animator;
        private static readonly int JUMP = Animator.StringToHash("jump");
        private static readonly int RUN = Animator.StringToHash("run");
        private bool toBeDisabled;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            // initial flip if needed
            if (facingRight && Math.Sign(transform.localScale.x) < 0)
                flip();
        }

        private bool isGrounded() {
            var colliderBot = Physics2D
                .Raycast(rayOrigin.transform.position, Vector2.down, rayCheckDistance, rayLayerMask)
                .collider;
            if (colliderBot != null) {
                return true;
            }

            // left
            colliderBot = Physics2D.Raycast(bottomLeft.transform.position, Vector2.down, rayCheckDistance, rayLayerMask)
                .collider;
            if (colliderBot != null) {
                return true;
            }

            // right
            colliderBot = Physics2D
                .Raycast(bottomRight.transform.position, Vector2.down, rayCheckDistance, rayLayerMask)
                .collider;

            return colliderBot != null;
        }

        private bool isGroundedDeadBody() {
            var colliderBot = Physics2D
                .Raycast(rayOrigin.transform.position, Vector2.down, rayCheckDistance, rayLayerMaskDead)
                .collider;
            return colliderBot != null;
        }

        public void prepareDisable() {
            toBeDisabled = true;
        }

        private void FixedUpdate() {
            if (toBeDisabled) {
                // wait for pig to touch ground, then static it
                if (!isGroundedDeadBody()) return;
                var rb2d = GetComponent<Rigidbody2D>();
//                rb2d.bodyType = RigidbodyType2D.Kinematic;
                rb2d.constraints = RigidbodyConstraints2D.FreezePositionX |
                                   RigidbodyConstraints2D.FreezeRotation;
                enabled = false;
                return;
            }

            var x = Input.GetAxis("Horizontal");
            if (Input.GetButtonDown("Jump")) {
                if (isGrounded()) {
                    var rbVelocity = rb.velocity;
                    rbVelocity.y = jump;
                    rb.velocity = rbVelocity;
//                    rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                    animator.SetTrigger(JUMP);
                }
            }

            var velocity = rb.velocity;
            velocity = new Vector3(x * speed, velocity.y, 0);
            rb.velocity = velocity;
            if (Mathf.Abs(velocity.y) > 0f) {
                animator.SetTrigger(JUMP);
            } else {
                animator.SetBool(RUN, velocity.sqrMagnitude > 0);
            }

            if (rb.velocity.x > 0 && !facingRight) {
                flip();
            } else if (rb.velocity.x < 0 && facingRight) {
                flip();
            }
        }

        private void flip() {
            facingRight = !facingRight;

            var transform1 = transform;
            var theScale = transform1.localScale;
            theScale.x = Math.Abs(theScale.x) * (facingRight ? 1 : -1);
            transform1.localScale = theScale;
        }
    }
}