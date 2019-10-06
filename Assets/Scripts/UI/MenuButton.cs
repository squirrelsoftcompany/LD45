using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public GameObject menu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Pause"))
        {
            if (Time.timeScale > 0)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }
    
    public void Resume()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }
    
    public void RestartLevel()
    {
        // restart level
    }
    
    public void RestartGame()
    {
        // restart level
    }
}
