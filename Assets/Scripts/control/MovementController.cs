using System;
using UnityEngine;

namespace control {
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour {
        [SerializeField] private float speed;

        private Vector3 direction;
        private Rigidbody2D rb2D;
        private const double TOLERANCE = 0.0003;

        // Start is called before the first frame update
        private void Start() {
            rb2D = GetComponent<Rigidbody2D>();
        }

        public void jump() {
            Debug.Log("jump!");
            // TODO
        }

        public void suicide() {
            Debug.Log("suicide");
        }

        private void FixedUpdate() {
            var translationSecond = direction * speed;
            rb2D.MovePosition(transform.position + Time.fixedDeltaTime * translationSecond);
            if (direction.x < 0f) {
                var transform1 = transform;
                var transformLocalRotation = transform1.localRotation;
                transformLocalRotation.x = -transformLocalRotation.x;
                transform1.localRotation = transformLocalRotation;
            }
        }

        // Update is called once per frame
        private void Update() {
            var moveHorizontal = Input.GetAxis("Horizontal");
            var moveVertical = Input.GetAxis("Jump");

            if (Math.Abs(moveHorizontal) > TOLERANCE || Math.Abs(moveVertical) > TOLERANCE) {
                direction = new Vector3(moveHorizontal, moveVertical, 0f).normalized;
            }
        }

        public void OnFire() {
            jump();
        }

        void onSuicide() { }

        public void OnMove() {
            Debug.Log("MOVE! ");
        }

        public void OnCombustion() {
            Debug.Log("combustion");
        }
    }
}