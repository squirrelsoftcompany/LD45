using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameEventSystem {
    [AddComponentMenu("GameEventListener")]
    public class GameEventListener : MonoBehaviour {
        public GameEvent gameEvent;
        public UnityEvent response;
        public ResponseWithString responseForSentString;
        public ResponseWithInt responseForSentInt;
        public ResponseWithFloat responseForSentFloat;
        public ResponseWithBool responseForSentBool;
        public ResponseWithMonoBehaviour responseForSentMonoBehaviour;

        private void OnEnable() {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable() {
            gameEvent.UnregisterListener(this);
        }

        [ContextMenu("Raise Event")]
        public void OnEventRaised() {
            // default/generic
            if (response.GetPersistentEventCount() >= 1) {
                // always check if at least 1 object is listening for the event
                response.Invoke();
            }

            // string
            if (responseForSentString.GetPersistentEventCount() >= 1) {
                responseForSentString.Invoke(gameEvent.sentString);
            }

            // int
            if (responseForSentInt.GetPersistentEventCount() >= 1) {
                responseForSentInt.Invoke(gameEvent.sentInt);
            }

            // float
            if (responseForSentFloat.GetPersistentEventCount() >= 1) {
                responseForSentFloat.Invoke(gameEvent.sentFloat);
            }

            // bool
            if (responseForSentBool.GetPersistentEventCount() >= 1) {
                responseForSentBool.Invoke(gameEvent.sentBool);
            }

            // Flammable
            if (responseForSentMonoBehaviour.GetPersistentEventCount() >= 1) {
                responseForSentMonoBehaviour.Invoke(gameEvent.sentMonoBehaviour);
            }
        }
    }

    [Serializable]
    public class ResponseWithString : UnityEvent<string> { }

    [Serializable]
    public class ResponseWithInt : UnityEvent<int> { }

    [Serializable]
    public class ResponseWithFloat : UnityEvent<float> { }

    [Serializable]
    public class ResponseWithBool : UnityEvent<bool> { }

    [Serializable]
    public class ResponseWithMonoBehaviour : UnityEvent<MonoBehaviour> { }
}