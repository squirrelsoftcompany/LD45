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

        public void startFlammable()
        {
            pigList.add(this);
        }

        public void startFire() {
            animator.SetTrigger(BURN);
        }

        public void endBurn() {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            pigList.remove(this);
        }
    }
}