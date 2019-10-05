using UnityEngine;
using UnityEngine.InputSystem;

namespace control {
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour, InputMaster.IPlayerActions {
        [SerializeField] private float speed;
        private InputMaster controls;

        private Vector3 direction;
        private Rigidbody2D rb2D;

        // Start is called before the first frame update
        private void Start() {
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void Awake() {
            controls = new InputMaster();
            controls.Player.SetCallbacks(this);
//            controls.Player.Jump.performed += _ => jump();
//            controls.Player.Movement.
        }

//
        private void OnEnable() {
            controls.Player.Enable();
        }

        private void OnDisable() {
            controls.Player.Disable();
        }

        public void jump() {
            Debug.Log("jump!");
            // TODO
        }

        public void suicide() {
            Debug.Log("suicide");
        }

        public void move(InputAction.CallbackContext context) {
            // todo
            Debug.Log("Move: " + context);
        }

        private void FixedUpdate() {
//            var translationSecond = direction * speed;
//            rb2D.MovePosition(transform.position + Time.fixedDeltaTime * translationSecond);
//            if (direction.x < 0f) {
//                var transform1 = transform;
//                var transformLocalRotation = transform1.localRotation;
//                transformLocalRotation.x = -transformLocalRotation.x;
//                transform1.localRotation = transformLocalRotation;
//            }
        }

        // Update is called once per frame
        private void Update() {
//            var moveHorizontal = Input.GetAxis("Horizontal");
//            var moveVertical = Input.GetAxis("Jump");
//
//            direction = new Vector3(moveHorizontal, moveVertical, 0f).normalized;
        }

        public void OnJump(InputAction.CallbackContext context) {
            jump();
        }

        public void OnMovement(InputAction.CallbackContext context) {
            move(context);
        }

        public void OnSuicide(InputAction.CallbackContext context) {
            suicide();
        }

        public void OnCombustion(InputAction.CallbackContext context) {
            Debug.Log("combustion");
        }
    }
}