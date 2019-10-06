using control;
using GameEventSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Control {
    [RequireComponent(typeof(Collider2D))]
    public class Suicide : MonoBehaviour {
        [Title("Prefabs")]
        // new pig creation information
        [AssetsOnly]
        [Required]
        public GameObject pigPrefab;

        private GameObject _spawn;

        [Required] [AssetsOnly] [SerializeField]
        private GameObject pigSoulPrefab;

        private GameObject soulsContainer;

        [Title("Game Events")]
        // events
        public GameEvent onDeath;

        public GameEvent onRevive;

        [Title("For anim")] public float animationState = 0;
        public Vector3 initPosition;

        // working variable
        private GameObject _currentPig;
        private GameObject _mask;
        private GameObject _maskParent;
        private bool _dying;
        private Collider2D _maskCollider2D;
        private Rigidbody2D rb2D;
        private Movement _movement;
        private MaskMovement maskMovement;
        private static readonly int DEAD = Animator.StringToHash("dead");

        // Start is called before the first frame update
        private void Start() {
            _spawn = GameObject.FindWithTag("Respawn");
            soulsContainer = GameObject.FindWithTag("SoulsContainer");

            _mask = gameObject;
            _maskCollider2D = _mask.GetComponent<Collider2D>();
            maskMovement = GetComponent<MaskMovement>();
            rb2D = GetComponent<Rigidbody2D>();

            var parent = _mask.transform.parent;

            _maskParent = parent ? parent.gameObject : null;
            _currentPig = parent ? parent.parent.gameObject : null;
            _movement = _currentPig ? _currentPig.GetComponent<Movement>() : null;
        }

        // Update is called once per frame
        private void Update() {
            var suicideRequested = Input.GetButtonUp("Suicide"); // || !_currentPig;
            if (!suicideRequested && !_dying) return;

            if (suicideRequested && !_dying) {
                SuicidePig();
            } else {
                if (MaskTransferIteration()) {
                    TransferComplete();
                }
            }
        }

        private void TransferComplete() {
            _mask.transform.SetParent(_maskParent.transform);

            _dying = false;
            _maskCollider2D.enabled = true;
            _movement.enabled = true;

            // send event
            onRevive.Raise();
        }

        private bool MaskTransferIteration() {
            animationState += Time.deltaTime;
            _mask.transform.position = initPosition + Vector3.Slerp(Vector3.zero,
                                           _maskParent.transform.position - initPosition, animationState);

            return animationState >= 1;
        }

        public void SuicidePig() {
            // deactivate happyArea triggering
            _maskCollider2D.enabled = false;

            // "kill" previous pig
            if (_currentPig) killPig(_currentPig);

            // remove mask only movement
            removeMaskOnlyMove();

            initPosition = _mask.transform.position;
            animationState = 0;

            // create new pig...
            var scale = _currentPig ? _currentPig.transform.localScale : Vector3.one;
            _currentPig = Instantiate(pigPrefab, _spawn.transform.position, Quaternion.identity, _spawn.transform);
            _currentPig.transform.localScale = scale;

            // and retrieve some information
            _movement = _currentPig.GetComponent<Movement>();
            _movement.enabled = false;

            _maskParent = null;
            foreach (Transform child in _currentPig.transform) {
                if (!child.gameObject.CompareTag("MaskParent")) continue;

                _maskParent = child.gameObject;
                break;
            }

            Debug.Assert(_maskParent != null, "no mask parent found");

            _dying = true;
        }

        private void removeMaskOnlyMove() {
            maskMovement.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Mask");
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.simulated = false;
            rb2D.useFullKinematicContacts = false;
        }

        private void controlWithMask() {
            maskMovement.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("MaskAlone");
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.simulated = true;
            rb2D.useFullKinematicContacts = true;
        }

        private void killPig(GameObject pig) {
            // send event
            onDeath.Raise();

            // "kill" previous pig

            // Spawn the soul of the pig
            var pigSoul = Instantiate(pigSoulPrefab, _currentPig.transform.position, Quaternion.identity,
                soulsContainer.transform);
            pigSoul.name = "Soul";
            pigSoul.layer = LayerMask.NameToLayer("DisembodiedSoul");

            pig.layer = LayerMask.NameToLayer("Body");
            // deactivate previous controller
            pig.GetComponent<Movement>().prepareDisable();
            pig.GetComponent<Attack>().enabled = false;
            pig.GetComponent<Animator>().SetBool(DEAD, true);
            pig.GetComponent<Flammable>().enabled = true;
        }
    }
}