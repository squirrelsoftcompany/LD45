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
            var vectorDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

//            var theSpeed = Time.fixedDeltaTime * speed;
            rb2D.velocity = vectorDir * speed; //* theSpeed;

            if (Mathf.Abs(vectorDir.x) > 0.1) {
                var scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * Mathf.Sign(vectorDir.x);
                transform.localScale = scale;
            }
        }
    }
}