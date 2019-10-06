using Control;
using GameEventSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Behaviour {
    public class FogArea : MonoBehaviour {
        [Required] [SerializeField] private GameEvent onEnter, onExit;

        private void OnTriggerEnter2D(Collider2D other) {
            var soul = other.GetComponent<SoulBehaviour>();
            if (soul) {
                soul.setFog(true);
            } else if (other.GetComponent<Suicide>()) {
                onEnter.Raise();
            }
        }

        private void OnTriggerStay2D(Collider2D other) {
            var soul = other.GetComponent<SoulBehaviour>();
            if (soul) {
                soul.setFog(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var soul = other.GetComponent<SoulBehaviour>();
            if (soul) {
                soul.setFog(false);
            } else if (other.GetComponent<Suicide>()) {
                onExit.Raise();
            }
        }
    }
}