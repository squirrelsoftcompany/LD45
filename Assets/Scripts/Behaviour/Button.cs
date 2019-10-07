using System;
//using UnityEditor.Animations;
using UnityEngine;

namespace Behaviour
{
    public class Button : MonoBehaviour
    {
        public abstract class Buttonable : MonoBehaviour
        {
            public abstract void ButtonateMe();
            public abstract void UnbuttonateMe();
        }

        public Buttonable[] buttonable;
        private Animator _anim;
        
        private static readonly int Down = Animator.StringToHash("Down");
        private static readonly int Default = Animator.StringToHash("Default");

        // Start is called before the first frame update
        void Start()
        {
            _anim = transform.parent.GetComponent<Animator>();
        }

        private int _triggerCount = 0;
        private void OnTriggerEnter2D(Collider2D other)
        {
            _triggerCount++;
            if (_triggerCount > 1) return;
            
            _anim.SetTrigger(Down);
            foreach (Buttonable button in buttonable)
                button?.ButtonateMe();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _triggerCount--;
            if (_triggerCount > 0) return;
            
            _anim.SetTrigger(Default);
            foreach(Buttonable button in buttonable)
                button?.UnbuttonateMe();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            
            foreach (var b in buttonable)
                Gizmos.DrawLine(transform.position, b.transform.position);
        }
    }
}
