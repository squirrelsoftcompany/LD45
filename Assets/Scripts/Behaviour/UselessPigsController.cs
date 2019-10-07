using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Behaviour {
    public class UselessPigsController : MonoBehaviour {
        private List<GameObject> pigs;
        [SerializeField] [Range(0f, 1f)] private float maxTimeBetweenAnim;
        [SerializeField] [Range(0f, 1f)] private float alphaColorOff;

        private bool on;

        [AssetsOnly] [Required] [SerializeField]
        private GameObject uselessPigPrefab;

        [SerializeField] [Required] private int nbPigs;

        private readonly Color[] colors = {
            new Color32(255, 255, 255, 204),
            new Color32(135, 179, 141, 204), new Color32(34, 3, 31, 204), new Color32(204, 118, 161, 204),
            new Color32(221, 146, 150, 204),
            new Color32(242, 183, 198, 204),
            new Color32(250, 179, 169, 204),
            new Color32(255, 202, 212, 204),
            new Color32(247, 175, 157, 204),
            new Color32(239, 189, 235, 204),
            new Color32(122, 89, 128, 204),
            new Color32(237, 191, 198, 204)
        };

        private static readonly int RUNNING_AROUND = Animator.StringToHash("runningAround");

        // Start is called before the first frame update
        private void Start() {
            pigs = new List<GameObject>();
            var transform1 = transform;
            for (var i = 0; i < nbPigs; i++) {
                pigs.Add(Instantiate(uselessPigPrefab, transform1.position, Quaternion.identity, transform1));
            }

            StartCoroutine(cascadeStartAnimation());
        }

        private IEnumerator cascadeStartAnimation() {
            foreach (var pig in pigs) {
                pig.GetComponent<Animator>().SetTrigger(RUNNING_AROUND);
                var spriteRenderer = pig.GetComponent<SpriteRenderer>();
                var color = colors[Random.Range(0, colors.Length)];
                color.a = on ? 1f : alphaColorOff;
                spriteRenderer.color = color;
                yield return new WaitForSeconds(Random.value * maxTimeBetweenAnim);
            }
        }

        public void switchOff() {
            Debug.Log("Switch off");
            on = false;
            foreach (var pig in pigs) {
                var spriteRenderer = pig.GetComponent<SpriteRenderer>();
                var color = spriteRenderer.color;
                color.a = alphaColorOff;
                spriteRenderer.color = color;
            }
        }

        public void lightItUp() {
            Debug.Log("Light up spawn");
            on = true;
            foreach (var pig in pigs) {
                var spriteRenderer = pig.GetComponent<SpriteRenderer>();
                var color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }
        }
    }
}