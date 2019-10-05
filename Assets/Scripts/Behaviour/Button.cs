﻿using System;
using UnityEditor.Animations;
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

        public Buttonable buttonable;
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
            buttonable?.ButtonateMe();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _triggerCount--;
            if (_triggerCount > 0) return;
            
            _anim.SetTrigger(Default);
            buttonable?.UnbuttonateMe();
        }
    }
}