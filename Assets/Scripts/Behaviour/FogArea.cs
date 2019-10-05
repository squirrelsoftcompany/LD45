using GameEventSystem;
using UnityEngine;

namespace Behaviour {
    public class FogArea : MonoBehaviour {
        [SerializeField] private GameEvent onEnter, onExit;

        private void OnTriggerEnter2D(Collider2D other) {
            onEnter.Raise();
        }

        private void OnTriggerExit2D(Collider2D other) {
            onExit.Raise();
        }
    }
}