using Sirenix.OdinInspector;
using UnityEngine;

namespace control {
    [RequireComponent(typeof(Animator))]
    public class Flammable : MonoBehaviour {
        private Animator animator;
        [SerializeField] private PigList pigList;
        private static readonly int BURN = Animator.StringToHash("burn");

        [Required] [SceneObjectsOnly] [SerializeField]
        private Light lightSelect;

        [Required] [SceneObjectsOnly] [SerializeField]
        private MeshRenderer instructionBurn;

        private bool firstTime;

        private void Start() {
            animator = GetComponent<Animator>();
            firstTime = PlayerPrefs.GetInt("FirstTime", 1) == 1;
        }

        public void startFlammable(bool isABody) {
            pigList.add(this, isABody);
        }

        public void startFire() {
            deselect();
            animator.SetTrigger(BURN);
            firstTime = false;
            PlayerPrefs.SetInt("FirstTime", 0);
        }

        public void endBurn() {
            Destroy(gameObject);
        }

        private void OnDestroy() {
            pigList.remove(this);
        }

        public void deselect() {
            // TODO de-highlight this pig
//            GameObject.FindGameObjectWithTag("LightSelect").SetActive(false);
//            GameObject.FindGameObjectWithTag("InstructionBurn").SetActive(false);

            lightSelect.enabled = false;
            instructionBurn.enabled = false;
        }

        public void select() {
            // TODO highlight this pig
            lightSelect.enabled = true;
            if (firstTime) {
//                GameObject.FindGameObjectWithTag("InstructionBurn").SetActive(true);

                instructionBurn.enabled = true;
            }
        }
    }
}