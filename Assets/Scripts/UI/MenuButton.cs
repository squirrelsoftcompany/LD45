using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MenuButton : MonoBehaviour
    {
        [Header("Menus")]
        public GameObject menu;
        public GameObject header;
        public GameObject finalScreen;

        [Header("Customize final screen")]
        public TextMeshProUGUI finalSentence;
        public TextMeshProUGUI nextButtonText;

        [Header("Stats")]
        public Stats.Stats levelStats;
        public Stats.Stats gameStats;
        
        private bool _paused = false;
        private bool _levelFinished = false;
        private int _buildIndex;

        private void Start()
        {
            // Customize final screen
            _buildIndex = SceneManager.GetActiveScene().buildIndex;
            var str = SceneManager.sceneCountInBuildSettings == _buildIndex + 1 ? "level "+_buildIndex : "last level";
            str = _buildIndex == 1 ? "first level" : str;

            finalSentence.text = string.Format(finalSentence.text, str);
            
            if (_buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
                nextButtonText.text = "ESCAPE";

            // init game/level stats
            levelStats.time = 0;
            levelStats.bodyCount = 0;
            if (_buildIndex == 1)
            {
                gameStats.time = 0;
                gameStats.bodyCount = 0;
            }
        }

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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // current level
        }
    
        public void RestartGame()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(1); // first level
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
            Time.timeScale = 1;
            
            if (SceneManager.sceneCountInBuildSettings == _buildIndex + 1)
                SceneManager.LoadScene(0); // menu
            else
                SceneManager.LoadScene(_buildIndex + 1); // next level
        }
    }
}
