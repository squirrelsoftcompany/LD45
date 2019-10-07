using UnityEngine;

namespace test {
    public class EventDebugListener : MonoBehaviour {
        public void onCleaverEnterPig() {
            Debug.Log("onCleaverEnterPig");
        }

        public void onDeath() {
            Debug.Log("onDeath");
        }

        public void onEnterFog() {
            Debug.Log("onEnterFog");
        }

        public void onExitFog() {
            Debug.Log("onExitFog");
        }

        public void onEnterHappy() {
            Debug.Log("onEnterHappy");
        }

        public void onExitHappy() {
            Debug.Log("onExitHappy");
        }

        public void onPigEnterSaw() {
            Debug.Log("onPigEnterSaw");
        }

        public void onRevive() {
            Debug.Log("onRevive");
        }

        public void onWin() {
            Debug.Log("onWin");
        }
    }
}