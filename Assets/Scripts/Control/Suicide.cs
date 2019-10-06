using control;
using UnityEngine;

namespace Control {
    [RequireComponent(typeof(Collider2D))]
    public class Suicide : MonoBehaviour {
        // new pig creation information
        public GameObject pigPrefab;
        private GameObject _spawn;

        // working variable
        private GameObject _currentPig;
        private GameObject _mask;
        private GameObject _maskParent;
        private bool _dying;
        private Collider2D _maskCollider2D;
        private Movement _movement;
        private static readonly int DEAD = Animator.StringToHash("dead");
        private bool fog;

        // Start is called before the first frame update
        private void Start() {
            _spawn = GameObject.FindWithTag("Respawn");

            _mask = gameObject;
            _maskCollider2D = _mask.GetComponent<Collider2D>();

            _maskParent = _mask.transform.parent.gameObject;
            _currentPig = _maskParent.transform.parent.gameObject;
            _movement = _currentPig.GetComponent<Movement>();
        }

        // Update is called once per frame
        private void Update() {
            bool suicideRequested = Input.GetButtonUp("Suicide");
            if (!suicideRequested && !_dying) return;

            if (suicideRequested && !_dying)
            {
                SuicidePig();
            }
            else
            {
                if (MaskTransferIteration())
                {
                    TransferComplete();
                }
            }
        }

        public void setFog(bool theFog) {
            fog = theFog;
        }

        private void TransferComplete()
        {
            _mask.transform.SetParent(_maskParent.transform);

            _dying = false;
            _maskCollider2D.enabled = true;
            _movement.enabled = true;
        }

        public float animationState = 0;
        public Vector3 initPosition;
        private bool MaskTransferIteration()
        {
            animationState += Time.deltaTime;
            _mask.transform.position = initPosition + Vector3.Slerp(Vector3.zero, _maskParent.transform.position - initPosition, animationState);
            
            return animationState >= 1;
        }

        public void SuicidePig()
        {
            // deactivate happyArea triggering
            _maskCollider2D.enabled = false;

            // "kill" previous pig
            killPig(_currentPig);
            
            initPosition = _mask.transform.position;
            animationState = 0;
            
            // create new pig...
            var scale = _currentPig.transform.localScale;
            _currentPig = Instantiate(pigPrefab, _spawn.transform.position, Quaternion.identity, _spawn.transform);
            _currentPig.transform.localScale = scale;

            // and retrieve some information
            _movement = _currentPig.GetComponent<Movement>();
            _movement.enabled = false;

            _maskParent = null;
            foreach (Transform child in _currentPig.transform)
            {
                if (!child.gameObject.CompareTag("MaskParent")) continue;

                _maskParent = child.gameObject;
                break;
            }

            Debug.Assert(_maskParent != null, "no mask parent found");

            _dying = true;
        }

        private void killPig(GameObject pig) {
            // "kill" previous pig
            var rb2D = pig.GetComponent<Rigidbody2D>();
            if (fog) {
                rb2D.gravityScale = 0;
                rb2D.bodyType = RigidbodyType2D.Static;
                // TODO FIXME instantiate ghost right here, but let the body pig get to the floor
            }
            // TODO wait for pig to touch ground, then static it

            pig.GetComponent<Animator>().SetBool(DEAD, true);
            pig.GetComponent<Flammable>().enabled = true;
            pig.GetComponent<Attack>().enabled = false;
            // deactivate previous controller
            pig.GetComponent<Movement>().enabled = false;
        }
    }
}