using UnityEngine;

namespace control {
    public class MaskMovement : MonoBehaviour {
        [SerializeField] private float speed;

        private void FixedUpdate() {
            var vectorDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            var transform1 = transform;
            var position = transform1.position;
            var theSpeed = Time.deltaTime * speed;
            position.x += vectorDir.x * theSpeed;
            position.y += vectorDir.y * theSpeed;

            transform1.position = position;
        }
    }
}