using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

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

        private int firstTime;
        [SerializeField] private int maxTimeTuto = 3;

        private void Start() {
            animator = GetComponent<Animator>();
            firstTime = PlayerPrefs.GetInt("FirstTime", 0);
        }

        public void startFlammable(bool isABody) {
            pigList.add(this, isABody);
            if (Mathf.Sign(instructionBurn.transform.lossyScale.x) < 0)
            {
                var localScale = instructionBurn.transform.localScale;
                instructionBurn.transform.localScale = new Vector3(-Mathf.Abs(localScale.x), localScale.y, localScale.z);
            }
        }

        public void startFire() {
            deselect();
            animator.SetTrigger(BURN);
            firstTime++;
            PlayerPrefs.SetInt("FirstTime", firstTime);
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
            if (firstTime < maxTimeTuto) {
//                GameObject.FindGameObjectWithTag("InstructionBurn").SetActive(true);

                instructionBurn.enabled = true;
            }
        }
    }
}