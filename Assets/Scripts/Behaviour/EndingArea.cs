using GameEventSystem;
using UnityEngine;

namespace Behaviour
{
    [RequireComponent(typeof(Collider2D))]
    public class EndingArea : MonoBehaviour
    {
        public GameEvent onEnter;

        private void OnTriggerEnter2D(Collider2D other)
        {
            onEnter.Raise();
        }
        
        private void OnDrawGizmos() {
            var color = Color.cyan;
            color.a = 0.3f;
            Gizmos.color = color;
            Gizmos.DrawCube(transform.position, transform.lossyScale);
        }
    }
}
