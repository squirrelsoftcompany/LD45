﻿using Sirenix.OdinInspector;
using UnityEngine;

namespace Behaviour {
    [RequireComponent(typeof(CircleCollider2D))]
    public class Spawn : MonoBehaviour {
        [SerializeField] private float range;

        private bool active;

        [Required] [SceneObjectsOnly] [SerializeField]
        private GameObject instructionText;

        [Required] [SerializeField] private Light lightSpawn;

        public bool Active => active;

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
            var position1 = transform.position;
            Gizmos.DrawSphere(position1, range);

            Gizmos.color = Color.blue;
            var position = position1;
            Gizmos.DrawLine(position - Vector3.left * 3,
                position + Vector3.left * 3);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            // A mask alone enters this area so light it up!
            instructionText.SetActive(true);
            controller.lightItUp();
            lightSpawn.enabled = true;
            active = true;
        }

        private void OnTriggerExit2D(Collider2D other) {
            // 
            instructionText.SetActive(false);
            controller.switchOff();
            lightSpawn.enabled = false;
            active = false;
        }
    }
}