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
        private Possess possess;
        private static readonly int DEAD = Animator.StringToHash("dead");

        public bool canSuicide = false;

        // Start is called before the first frame update
        private void Start() {
            _spawn = GameObject.FindWithTag("Respawn");
            soulsContainer = GameObject.FindWithTag("SoulsContainer");
            possess = GetComponent<Possess>();

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
            if (!suicideRequested) return;

            if (canSuicide) {
                SuicidePig();
            }
        }

//        private void TransferComplete() {
//            _mask.transform.SetParent(_maskParent.transform);
//
//            _dying = false;
//            _maskCollider2D.enabled = true;
//            _movement.enabled = true;
//
//            // send event
//            onRevive.Raise();
//        }

//        private bool MaskTransferIteration() {
//            animationState += Time.deltaTime;
//            _mask.transform.position = initPosition + Vector3.Slerp(Vector3.zero,
//                                           _maskParent.transform.position - initPosition, animationState);
//
//            return animationState >= 1;
//        }

        public void SuicidePig() {
            canSuicide = false;
            // deactivate happyArea triggering

            // "kill" previous pig
            killPig();

            // remove mask only movement

//            removeMaskOnlyMove();
            controlWithMaskOnly();
        }


        private void controlWithMaskOnly() {
            maskMovement.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("MaskAlone");
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.simulated = true;
            rb2D.useFullKinematicContacts = true;
            _maskCollider2D.enabled = true;
            possess.canPossess = true;
        }

        private void removeMaskOnlyMove() {
            maskMovement.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Mask");
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.simulated = false;
            rb2D.useFullKinematicContacts = false;
        }

        private void killPig() {
            // send event
            onDeath.Raise();

            // Find pig
            var pig = transform.parent.parent.gameObject;

            // "kill" previous pig

            // Spawn the soul of the pig
            var pigSoul = Instantiate(pigSoulPrefab, pig.transform.position, Quaternion.identity,
                soulsContainer.transform);
            pigSoul.name = "Soul";
            pigSoul.layer = LayerMask.NameToLayer("DisembodiedSoul");

            pig.layer = LayerMask.NameToLayer("Body");
            // deactivate previous controller
            pig.GetComponent<Movement>().prepareDisable();
            pig.GetComponent<Attack>().enabled = false;
            pig.GetComponent<Animator>().SetBool(DEAD, true);
            var flammable = pig.GetComponent<Flammable>();
            flammable.enabled = true;
            flammable.startFlammable();
        }
    }
}