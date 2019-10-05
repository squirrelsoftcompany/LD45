using UnityEngine;

namespace control {
    [RequireComponent(typeof(Animator))]
    public class Flammable : MonoBehaviour {
        private Animator animator;
        [SerializeField] private PigList pigList;
        private static readonly int BURN = Animator.StringToHash("burn");

        private void Start() {
            animator = GetComponent<Animator>();
        }

        private void OnEnable() {
            pigList.add(GetComponent<Flammable>());
        }

        private void OnDisable() {
            pigList.remove(GetComponent<Flammable>());
        }

        public void startFire() {
            animator.SetTrigger(BURN);
        }

        public void endBurn() {
            pigList.remove(this);
            Destroy(gameObject);
        }
    }
}