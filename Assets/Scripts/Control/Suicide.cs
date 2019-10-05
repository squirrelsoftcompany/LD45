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
        private GameObject _currentPig;
        private GameObject _mask;

        private bool _dying;
        
        // Start is called before the first frame update
        private void Start()
        {
            test = false;
            _currentPig = GameObject.FindWithTag("Player");
            _mask = GameObject.FindWithTag("Mask");
        }

        // Update is called once per frame
        private void Update()
        {
            if (!test && !_dying) return;

            if (test && !_dying)
            {
                // todo deactivate previous controller
                // todo "kill" previous pig
                var newPig = Instantiate(pigPrefab, spawn.transform.position, Quaternion.identity, spawn.transform);

                GameObject maskParent = null;
                foreach (Transform child in newPig.transform)
                {
                    if (!child.gameObject.CompareTag("MaskParent")) continue;

                    maskParent = child.gameObject;
                    break;
                }
                
                if (maskParent != null)
                {
                    _mask.transform.SetParent(maskParent.transform);
                }

                _dying = true;
            }

            if (_dying)
            {
                _mask.transform.localPosition = Vector3.Slerp(_mask.transform.localPosition, Vector3.zero, Time.deltaTime * 2);
                if (_mask.transform.localPosition.magnitude <= 0.01) _dying = false;
            }

            test = false;
        }
    }
}
