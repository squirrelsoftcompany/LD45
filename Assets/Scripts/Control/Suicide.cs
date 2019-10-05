using System;
using System.Numerics;
using control;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Control {
    [RequireComponent(typeof(Collider2D))]
    public class Suicide : MonoBehaviour {
        // new pig creation information
        public GameObject pigPrefab;
        private GameObject _spawn;

        // working variable
        private GameObject _currentPig;
        private GameObject _mask;
        private GameObject _maskParent;
        private bool _dying;
        private Collider2D _maskCollider2D;
        private Movement _movement;
        private static readonly int DEAD = Animator.StringToHash("dead");

        // Start is called before the first frame update
        private void Start() {
            _spawn = GameObject.FindWithTag("Respawn");

            _mask = gameObject;
            _maskCollider2D = _mask.GetComponent<Collider2D>();

            _maskParent = _mask.transform.parent.gameObject;
            _currentPig = _maskParent.transform.parent.gameObject;
            _movement = _currentPig.GetComponent<Movement>();
        }

        // Update is called once per frame
        private void Update() {
            bool suicideRequested = Input.GetButtonUp("Suicide");
            if (!suicideRequested && !_dying) return;

            if (suicideRequested && !_dying)
            {
                SuicidePig();
            }
            else
            {
                if (MaskTransferIteration())
                {
                    TransferComplete();
                }
            }
        }

        private void TransferComplete()
        {
            _mask.transform.SetParent(_maskParent.transform);

            _dying = false;
            _maskCollider2D.enabled = true;
            _movement.enabled = true;
        }

        private bool MaskTransferIteration()
        {
            var position = _mask.transform.position;
            position = Vector3.Slerp(position, _maskParent.transform.position, Time.deltaTime * 2);
            _mask.transform.position = position;

            return Vector3.Distance(position, _maskParent.transform.position) <= 0.01;
        }

        private void SuicidePig()
        {
            // deactivate happyArea triggering
            _maskCollider2D.enabled = false;

            // "kill" previous pig
            killPig(_currentPig);

            // create new pig...
            var scale = _currentPig.transform.localScale;
            _currentPig = Instantiate(pigPrefab, _spawn.transform.position, Quaternion.identity, _spawn.transform);
            _currentPig.transform.localScale = scale;

            // and retrieve some information
            _movement = _currentPig.GetComponent<Movement>();
            _movement.enabled = false;

            _maskParent = null;
            foreach (Transform child in _currentPig.transform)
            {
                if (!child.gameObject.CompareTag("MaskParent")) continue;

                _maskParent = child.gameObject;
                break;
            }

            Debug.Assert(_maskParent != null, "no mask parent found");

            _dying = true;
        }

        private void killPig(GameObject pig) {
            // "kill" previous pig
            pig.GetComponent<Animator>().SetBool(DEAD, true);
            pig.GetComponent<Flammable>().enabled = true;
            pig.GetComponent<Attack>().enabled = false;
            // deactivate previous controller
            pig.GetComponent<Movement>().enabled = false;
        }
    }
}