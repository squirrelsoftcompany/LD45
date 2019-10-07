using System;
using UnityEngine;

namespace control {
    public class Attack : MonoBehaviour {
        [SerializeField] private PigList pigList;
        [SerializeField] private float attackRange;

        private Flammable flammableClosest;

        public void unselectAll() {
            if (flammableClosest != null) {
                flammableClosest.deselect();
                flammableClosest = null;
            }
        }

        // Update is called once per frame
        private void Update() {
            var closest = getClosestRangedPig();
            if (!closest) {
                if (flammableClosest != null) {
                    flammableClosest.deselect();
                    flammableClosest = null;
                }

                return;
            }

            if (flammableClosest != closest) {
                if (flammableClosest != null) {
                    flammableClosest.deselect();
                }

                flammableClosest = closest;
                flammableClosest.select();
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                // kill it with fire!!
                closest.startFire();
            }
        }

        private void OnDrawGizmos() {
            if (enabled) {
                var color = Color.cyan;
                color.a = 0.3f;
                Gizmos.color = color;
                var position1 = transform.position;
                Gizmos.DrawSphere(position1, attackRange);
            }
        }

        private Flammable getClosestRangedPig() {
            Flammable best = null;
            var closestDistSqr = Mathf.Infinity;
            var currentPosition = transform.position;
            foreach (var flammable in pigList.pigs) {
                var directionToTarget = flammable.transform.position - currentPosition;
                var distSqrtToTarget = new Vector2(directionToTarget.x, directionToTarget.y).sqrMagnitude;
                if (!(distSqrtToTarget < closestDistSqr) ||
                    !(distSqrtToTarget < attackRange * attackRange)) continue;
                closestDistSqr = distSqrtToTarget;
                best = flammable;
            }

            return best;
        }
    }
}