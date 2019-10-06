using Sirenix.OdinInspector;
using UnityEngine;

namespace Behaviour {
    [RequireComponent(typeof(Rigidbody2D))]
    public class SoulBehaviour : MonoBehaviour {
        [SerializeField] [ValidateInput("NotZero", "Please specify a speed!")]
        private float speedUp;

        private bool fog;

        private bool NotZero(float input) {
            return input != 0;
        }

        public void setFog(bool theFog) {
            fog = theFog;
        }

        private void FixedUpdate() {
            var transform1 = transform;
            var pos = transform1.position;
            pos.y += speedUp * Time.fixedDeltaTime;
            transform1.position = pos;

            if (fog) {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.layer = LayerMask.NameToLayer("Soul");
                enabled = false;
            }
        }
    }
}