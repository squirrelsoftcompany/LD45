using System.Collections;
using GameEventSystem;
using UnityEngine;

namespace control {
    [RequireComponent(typeof(Animator))]
    public class Flammable : MonoBehaviour {
        [SerializeField] private float timeToBurn = 3000;
        [SerializeField] private GameEvent pigRemoved;
        private Animator animator;
        private float timeLeftBurning;
        private static readonly int BURN = Animator.StringToHash("burn");

        private void Start() {
            animator = GetComponent<Animator>();
        }

        public void startFire() {
            timeLeftBurning = timeToBurn;
            animator.SetTrigger(BURN);

            StartCoroutine(burn());
        }

        private IEnumerator burn() {
            while (timeLeftBurning > 0f) {
                timeLeftBurning -= Time.deltaTime;
                yield return null;
            }
        }

        public void endBurn() {
            Destroy(gameObject);
        }
    }
}