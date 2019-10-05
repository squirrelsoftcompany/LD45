using System;
using UnityEngine;

namespace control {
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour {
        [SerializeField] private float speed;

        private Vector3 direction;
        private Rigidbody2D rb2D;

        // Start is called before the first frame update
        private void Start() {
            rb2D = GetComponent<Rigidbody2D>();
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

            direction = new Vector3(moveHorizontal, moveVertical, 0f).normalized;
        }
    }
}