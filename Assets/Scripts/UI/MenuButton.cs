using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MenuButton : MonoBehaviour
    {
        public GameObject menu;
        public GameObject header;
        public GameObject finalScreen;

        private bool _paused = false;
        private bool _levelFinished = false;
    
        // Update is called once per frame
        void Update()
        {
            if (_levelFinished) return;
        
            if (Input.GetButtonUp("Pause"))
            {
                if (!_paused)
                    Pause();
                else
                    Resume();
            }
        }

        //*** Menu's buttons
    
        public void Pause()
        {
            _paused = true;
            Time.timeScale = 0;
            menu.SetActive(true);
        }

        public void Resume()
        {
            _paused = false;
            Time.timeScale = 1;
            menu.SetActive(false);
        }

        public void RestartLevel()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    
        public void RestartGame()
        {
            // Time.timeScale = 1;
            // SceneManager.LoadScene(FirstLevelName);
        }
    
        //***  final screen's button

        public void LevelFinished()
        {
            _levelFinished = true;
            Time.timeScale = 0;
            finalScreen.SetActive(true);
            header.SetActive(false);
            menu.SetActive(false);
        }
        public void NextLevel()
        {
            // Time.timeScale = 1;
            // SceneManager.LoadScene(nextLevelName);
        }
    }
}
