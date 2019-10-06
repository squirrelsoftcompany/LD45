using UnityEngine;

namespace control {
    [RequireComponent(typeof(Collider2D))]
    public class MaskMovement : MonoBehaviour {
        [SerializeField] private float speed;
        private Rigidbody2D rb2D;

        private void Start() {
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            var vectorDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            var theSpeed = Time.fixedDeltaTime * speed;
            rb2D.velocity = vectorDir * theSpeed;
        }
    }
}