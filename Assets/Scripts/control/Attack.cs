using UnityEngine;

namespace control {
    public class Attack : MonoBehaviour {
        [SerializeField] private PigList pigList;
        [SerializeField] private float attackRange;

        // Update is called once per frame
        private void Update() {
            if (Input.GetKeyDown(KeyCode.E)) {
                // kill it with fire!!
                getClosestRangedPig()?.startFire();
            }
        }

        private Flammable getClosestRangedPig() {
            Flammable best = null;
            var closestDistSqr = Mathf.Infinity;
            var currentPosition = transform.position;
            foreach (var flammable in pigList.pigs) {
                var directionToTarget = flammable.transform.position - currentPosition;
                var distSqrtToTarget = directionToTarget.sqrMagnitude;
                if (!(distSqrtToTarget < closestDistSqr) ||
                    !(distSqrtToTarget < attackRange * attackRange)) continue;
                closestDistSqr = distSqrtToTarget;
                best = flammable;
            }

            return best;
        }
    }
}