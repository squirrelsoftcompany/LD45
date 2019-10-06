using System.Collections;
using Control;
using GameEventSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace control {
    public class Possess : MonoBehaviour {
        [Required] [SceneObjectsOnly] [SerializeField]
        private GameObject[] spawns;

        [AssetsOnly] [Required] [SerializeField]
        private GameObject prefabPig;

        [SerializeField] private float range;

        [Required] [AssetsOnly] public GameEvent onRevive;

        private MaskMovement maskMovement;
        private Movement movePig;
        private Collider2D maskCollider2D;

        private Rigidbody2D rb2D;
        private GameObject maskParent;

        public bool canPossess = true;
        private Suicide suicide;

        private void Start() {
            maskMovement = GetComponent<MaskMovement>();
            rb2D = GetComponent<Rigidbody2D>();
            maskCollider2D = GetComponent<Collider2D>();
            suicide = GetComponent<Suicide>();

            spawns = GameObject.FindGameObjectsWithTag("Respawn");
            if (spawns.Length == 0) {
                Debug.LogError(
                    "Don't forget to mark spawn points with tag Respawn!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!'");
            }
        }

        // Update is called once per frame
        private void Update() {
            if (Input.GetButtonDown("Suicide") && canPossess) {
                // Possess
                var closestSpawn = getClosestRangedSpawn();
                if (closestSpawn) {
                    possess(closestSpawn);
                    canPossess = false;
                }
            }
        }

        private GameObject getClosestRangedSpawn() {
            GameObject best = null;
            var closestDistSqr = Mathf.Infinity;
            var currentPosition = transform.position;
            foreach (var spawn in spawns) {
                var directionToTarget = spawn.transform.position - currentPosition;
                var distSqrtToTarget = directionToTarget.sqrMagnitude;
                if (!(distSqrtToTarget < closestDistSqr) ||
                    !(distSqrtToTarget < range * range)) continue;
                closestDistSqr = distSqrtToTarget;
                best = spawn;
            }

            return best;
        }

        private void removeMaskOnlyMove() {
            maskMovement.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Mask");
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.simulated = false;
            rb2D.useFullKinematicContacts = false;
        }

        private void possess(GameObject spawn) {
            var piggy = Instantiate(prefabPig, spawn.transform.position, Quaternion.identity, spawn.transform);

            //
            movePig = piggy.GetComponent<Movement>();
            movePig.enabled = false;

            maskParent = null;
            foreach (Transform child in piggy.transform) {
                if (!child.gameObject.CompareTag("MaskParent")) continue;
                maskParent = child.gameObject;
                break;
            }

            removeMaskOnlyMove();
            StartCoroutine(moveMaskToPig(spawn.transform.position));
        }

        private IEnumerator moveMaskToPig(Vector3 spawnPos) {
            var animState = 0f;
            while (animState < 1) {
                transform.position =
                    spawnPos + Vector3.Slerp(Vector3.zero, maskParent.transform.position - spawnPos, animState);
                animState += Time.deltaTime;
                yield return null;
            }

            transform.SetParent(maskParent.transform);
            maskCollider2D.enabled = true;
            movePig.enabled = true;

            // Send event
            onRevive.Raise();
            suicide.canSuicide = true;
        }
    }
}