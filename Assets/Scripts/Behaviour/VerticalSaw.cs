using System.Collections;
using GameEventSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Behaviour {
    public class VerticalSaw : Button.Buttonable {
        public GameEvent onEnter;

        [SerializeField] private AnimationCurve mCurve;
        [SerializeField] private float mSize;
        [SerializeField] private float mDuration;
        [SerializeField] private bool mActivated = false;

        [SerializeField] [ValueDropdown("Orientation")]
        private bool orientation;

        private Animator animator;

        private Vector3 mInitialPosition;

        private static readonly int TURNING = Animator.StringToHash("turning");

        private static IEnumerable Orientation = new ValueDropdownList<bool> {
            {"Horizontal", true},
            {"Vertical", false}
        };

        private const float TILE_SIZE = .15f;

        // Start is called before the first frame update
        void Start() {
            mInitialPosition = transform.position;
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update() {
            if (!mActivated) return;
            if (orientation) {
                // horizontal
                transform.position = new Vector3(
                    mInitialPosition.x = mCurve.Evaluate(Time.time / mDuration) * TILE_SIZE * mSize,
                    mInitialPosition.y,
                    mInitialPosition.z);
            } else {
                // Vertical
                transform.position = new Vector3(mInitialPosition.x,
                    mInitialPosition.y + mCurve.Evaluate(Time.time / mDuration) * TILE_SIZE * mSize,
                    mInitialPosition.z);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            onEnter.Raise();
        }

        public override void ButtonateMe() {
            mActivated = false;
            animator.SetBool(TURNING, false);
        }

        public override void UnbuttonateMe() {
            mActivated = true;
            animator.SetBool(TURNING, true);
        }
    }
}