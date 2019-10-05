using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Control
{
    public class Suicide : MonoBehaviour
    {
        // new pig creation information
        public GameObject pigPrefab;
        public GameObject spawn;
        public bool test;

        // initialisation
        public GameObject currentPig;
        public GameObject mask;

        private bool dying;
        
        // Start is called before the first frame update
        private void Start()
        {
            test = false;
            currentPig = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        private void Update()
        {
            if (!test && !dying) return;

            if (test && !dying)
            {
                // todo deactivate previous controller
                // todo "kill" previous pig
                var newPig = Instantiate(pigPrefab, spawn.transform.position, Quaternion.identity, spawn.transform);

                GameObject maskParent = null;
                foreach (Transform child in newPig.transform)
                {
                    if (!child.gameObject.CompareTag("maskParent")) continue;

                    maskParent = child.gameObject;
                    break;
                }
                
                if (maskParent != null)
                {
                    mask.transform.SetParent(maskParent.transform);
                }

                dying = true;
            }

            if (dying)
            {
                mask.transform.localPosition = Vector3.Slerp(mask.transform.localPosition, Vector3.zero, Time.deltaTime * 2);
                if (mask.transform.localPosition.magnitude <= 0.01) dying = false;
            }

            test = false;
        }
    }
}
