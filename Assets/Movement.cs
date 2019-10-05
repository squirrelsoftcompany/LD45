using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Movement : MonoBehaviour {
    public float speed;
    public float jump;
    public GameObject rayOrigin;
    public float rayCheckDistance;
    Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool facingRight = false;
    private bool grounded = true;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        var theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}