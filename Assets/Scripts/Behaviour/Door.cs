using System;
using UnityEditor.Animations;
using UnityEngine;

namespace Behaviour
{
    public class Door : Button.Buttonable
    {
        private Animator _anim;
        private static readonly int Open = Animator.StringToHash("Open");
        private static readonly int Close = Animator.StringToHash("Close");

        // Start is called before the first frame update
        void Start()
        {
            _anim = GetComponent<Animator>();
        }

        public override void ButtonateMe()
        {
            _anim.SetTrigger(Open);
        }

        public override void UnbuttonateMe()
        {
            _anim.SetTrigger(Close);
        }
    }
}
