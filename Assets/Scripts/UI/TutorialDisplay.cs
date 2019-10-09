using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisplay : MonoBehaviour
{
    public GameObject text;
    public Stats.Stats stats;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (stats.bodyCount > 0) return;
        text.SetActive(true);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        text.SetActive(false);
    }
}
