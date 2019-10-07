using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PrincipalMenuButton : MonoBehaviour
    {
        public void StartGame()
        {
            // goto level1
            SceneManager.LoadScene(1);
        }
    }
}
