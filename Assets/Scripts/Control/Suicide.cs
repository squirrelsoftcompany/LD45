using System;
using System.Numerics;
using control;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Control
{
    [RequireComponent(typeof(Collider2D))]
    public class Suicide : MonoBehaviour
    {
        // new pig creation information
        public GameObject pigPrefab;
        public GameObject spawn;

        // initialisation
        private GameObject _currentPig;
        private GameObject _mask;
        private GameObject _maskParent;

        private bool _dying;
        private Collider2D _maskCollider2D;
        private Movement _movement;

        // Start is called before the first frame update
        private void Start()
        {
            _mask = gameObject;
            _maskCollider2D = _mask.GetComponent<Collider2D>();

            _maskParent = _mask.transform.parent.gameObject;
            _currentPig = _maskParent.transform.parent.gameObject;
            _movement = _currentPig.GetComponent<Movement>();
        }

        // Update is called once per frame
        private void Update()
        {
            bool suicideRequested = Input.GetButtonUp("Suicide");
            if (!suicideRequested && !_dying) return;

            if (suicideRequested && !_dying)
            {
                // deactivate happyArea triggering
                _maskCollider2D.enabled = false;
                
                // deactivate previous controller
                _movement.enabled = false;
                
                // todo "kill" previous pig

                _currentPig = Instantiate(pigPrefab, spawn.transform.position, Quaternion.identity, spawn.transform);
                _movement = _currentPig.GetComponent<Movement>();
                _movement.enabled = false;

                _maskParent = null;
                foreach (Transform child in _currentPig.transform)
                {
                    if (!child.gameObject.CompareTag("MaskParent")) continue;

                    _maskParent = child.gameObject;
                    break;
                }
                Debug.Assert(_maskParent, "no mask parent found");

                _dying = true;
            }

            if (!_dying) return;
            
            var position = _mask.transform.position;
            position = Vector3.Slerp(position, _maskParent.transform.position, Time.deltaTime * 2);
            _mask.transform.position = position;

            if (!(Vector3.Distance(position, _maskParent.transform.position) <= 0.01)) return;
            
            _mask.transform.SetParent(_maskParent.transform);
                    
            _dying = false;
            _maskCollider2D.enabled = true;
            _movement.enabled = true;
        }
    }
}
