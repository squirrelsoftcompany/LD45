using System.Collections.Generic;
using UnityEngine;

namespace control {
    public class Attack : MonoBehaviour {
        private List<Flammable> flammables;
        [SerializeField] private float attackRange;

        // Start is called before the first frame update
        void Start() { }


        // Update is called once per frame
        private void Update() {
            if (Input.GetKeyDown(KeyCode.E)) {
                // TODO kill it with fire!!
                getClosestRangedPig()?.startFire();
            }
        }

        public void addPig(MonoBehaviour flammable) {
            if (flammable is Flammable f) {
                flammables.Add(f);
            }
        }

        private Flammable getClosestRangedPig() {
            Flammable best = null;
            var closestDistSqr = Mathf.Infinity;
            var currentPosition = transform.position;
            foreach (var flammable in flammables) {
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