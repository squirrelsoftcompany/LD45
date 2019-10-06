using System.Collections;
using System.Collections.Generic;
using GameEventSystem;
using UnityEngine;


namespace Behaviour
{
    public class VerticalSaw : Button.Buttonable
    {

        public GameEvent onEnter;

        [SerializeField]
        private AnimationCurve mCurve;
        [SerializeField]
        private float mSize;
        [SerializeField]
        private float mDuration;
        [SerializeField]
        private bool mActivated = false;

        private Vector3 mInitialPosition;
        // Start is called before the first frame update

        void Start()
        {
            mInitialPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (mActivated)
                transform.position = new Vector3(mInitialPosition.x, mInitialPosition.y + mCurve.Evaluate(Time.time / mDuration) * 0.15f * mSize, mInitialPosition.z);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                onEnter.Raise();
            }
        }

        public override void ButtonateMe()
        {
            mActivated = false;
        }

        public override void UnbuttonateMe()
        {
            mActivated = true;
        }
    }
}