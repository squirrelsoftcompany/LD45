using System;
using GameEventSystem;
using UnityEngine;

namespace Behaviour {
    [RequireComponent(typeof(CircleCollider2D))]
    public class Spawn : MonoBehaviour {
        [SerializeField] private float range;

        public float Range => range;

        private CircleCollider2D circleCollider2D;
        private UselessPigsController controller;

        private void Start() {
            circleCollider2D = GetComponent<CircleCollider2D>();
            circleCollider2D.radius = range;
            controller = GetComponentInChildren<UselessPigsController>();
        }

        private void OnDrawGizmos() {
            var color = Color.magenta;
            color.a = 0.3f;
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, range);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            // A mask alone enters this area so light it up!
            controller.lightItUp();
        }

        private void OnTriggerExit2D(Collider2D other) {
            // 
            controller.switchOff();
        }
    }
}