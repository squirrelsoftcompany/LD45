using GameEventSystem;
using UnityEngine;

namespace Behaviour
{
    public class HappyArea : MonoBehaviour
    {
        public GameEvent onEnter;
        public GameEvent onExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            onEnter.Raise();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            onExit.Raise();
        }
    }
}
