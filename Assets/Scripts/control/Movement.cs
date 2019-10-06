using System;
using UnityEngine;

namespace control {
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class Movement : MonoBehaviour {
        public float speed;
        public float jump;
        public GameObject rayOrigin;
        public float rayCheckDistance;
        public LayerMask rayLayerMask;
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
            var res = colliderBot != null && !colliderBot.isTrigger;
            if (res) {
                Debug.Log(gameObject.name + " is Grounded by " + colliderBot.name);
            } else {
                Debug.Log(gameObject.name + " is NOT grounded");
            }

            return res;
        }

        public void prepareDisable() {
            toBeDisabled = true;
        }

        private void FixedUpdate() {
            if (toBeDisabled) {
                // wait for pig to touch ground, then static it
                if (!isGrounded()) return;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                enabled = false;
                return;
            }

            var x = Input.GetAxis("Horizontal");
            if (Input.GetAxis("Jump") > 0) {
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