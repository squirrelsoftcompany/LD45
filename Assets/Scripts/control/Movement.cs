using UnityEngine;

namespace control {
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class Movement : MonoBehaviour {
        public float speed;
        public float jump;
        public GameObject rayOrigin;
        public float rayCheckDistance;
        private Rigidbody2D rb;
        private bool facingRight = true;
        private Animator animator;
        private static readonly int JUMP = Animator.StringToHash("jump");
        private static readonly int RUN = Animator.StringToHash("run");

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private bool isGrounded() {
            return Physics2D.Raycast(rayOrigin.transform.position, Vector2.down, rayCheckDistance);
        }

        void FixedUpdate() {
            float x = Input.GetAxis("Horizontal");
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
//
//        private void OnCollisionEnter2D(Collision2D other) {
//            if (isGrounded()) {
//                grounded = true;
//            }
//        }
//
//        private void OnCollisionStay2D(Collision2D other) {
//            if (!grounded && other.gameObject.transform.position.y < transform.position.y) {
//                grounded = isGrounded();
//            }
//        }

        private void flip() {
            facingRight = !facingRight;

            var transform1 = transform;
            var theScale = transform1.localScale;
            theScale.x *= -1;
            transform1.localScale = theScale;
        }
    }
}