using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour {
    public float speed;
    public float jump;
    public GameObject rayOrigin;
    public float rayCheckDistance;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool grounded = true;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private bool isGrounded() {
        var hit = Physics2D.Raycast(rayOrigin.transform.position, Vector2.down, rayCheckDistance);
        return hit.collider != null;
    }

    void FixedUpdate() {
        float x = Input.GetAxis("Horizontal");
        if (Input.GetAxis("Jump") > 0) {
            if (grounded) {
                rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
                grounded = false;
            }
        }

        rb.velocity = new Vector3(x * speed, rb.velocity.y, 0);
        if (rb.velocity.x > 0 && !facingRight) {
            flip();
        } else if (rb.velocity.x < 0 && facingRight) {
            flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (isGrounded()) {
            grounded = true;
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (!grounded && isGrounded()) {
            grounded = true;
        }
    }

    private void flip() {
        facingRight = !facingRight;

        var transform1 = transform;
        var theScale = transform1.localScale;
        theScale.x *= -1;
        transform1.localScale = theScale;
    }
}