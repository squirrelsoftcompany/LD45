using GameEventSystem;
using UnityEngine;

namespace Behaviour
{
    public class EndingArea : MonoBehaviour
    {
        public GameEvent onEnter;

        private void OnTriggerEnter2D(Collider2D other)
        {
            onEnter.Raise();
        }
    }
}
