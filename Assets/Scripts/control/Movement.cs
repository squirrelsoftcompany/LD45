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
        private Animator animator;
        private static readonly int JUMP = Animator.StringToHash("jumping");
        private static readonly int RUN = Animator.StringToHash("run");
        private bool toBeDisabled;

        private bool _jumping = false;

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
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
            
            if (Input.GetAxis("Jump") > 0) {
                if (isGrounded()) {
                    var rbVelocity = rb.velocity;
                    rbVelocity.y = jump;
                    rb.velocity = rbVelocity;
                    _jumping = true;
                    animator.SetBool(JUMP, true);
                }
            }
            else if (_jumping && isGrounded())
            {
                _jumping = false;
                animator.SetBool(JUMP, false);
            }

            var h = Input.GetAxis("Horizontal");
            var velocity = rb.velocity;
            velocity = new Vector3( h * speed, velocity.y, 0);
            rb.velocity = velocity;
            
            animator.SetBool(RUN, Mathf.Abs(h) > 0f /*&& velocity.sqrMagnitude > 0.3f*/);

            // flip pig if necessary
            if (Mathf.Abs(rb.velocity.x) > 0.1f)
            {
                var scale = transform.localScale;
                scale.x = Math.Abs(scale.x) * Mathf.Sign(rb.velocity.x);
                transform.localScale = scale;
            }
        }
    }
}