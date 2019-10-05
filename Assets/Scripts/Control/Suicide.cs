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

        // initialisation
        private GameObject _currentPig;
        private GameObject _mask;

        private bool _dying;
        
        // Start is called before the first frame update
        private void Start()
        {
            _currentPig = GameObject.FindWithTag("Player");
            _mask = GameObject.FindWithTag("Mask");
        }

        // Update is called once per frame
        private void Update()
        {
            bool suicideRequested = Input.GetButtonUp("Suicide");
            if (!suicideRequested && !_dying) return;

            if (suicideRequested && !_dying)
            {
                // deactivate happyArea triggering
                _mask.GetComponent<Collider2D>().enabled = false;
                
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
                var localPosition = _mask.transform.localPosition;
                localPosition = Vector3.Slerp(localPosition, Vector3.zero, Time.deltaTime * 2);
                _mask.transform.localPosition = localPosition;

                if (localPosition.magnitude <= 0.01)
                {
                    _dying = false;
                    _mask.GetComponent<Collider2D>().enabled = true;
                }
            }
        }
    }
}
