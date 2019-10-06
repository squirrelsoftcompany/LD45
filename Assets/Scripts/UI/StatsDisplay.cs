using System;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace UI
{
    public class StatsDisplay : MonoBehaviour
    {
        public TextMeshProUGUI bodyCountText;
        public TextMeshProUGUI timeText;

        public Stats.Stats toDisplay;
        
        private void FixedUpdate()
        {
            bodyCountText.text = $"{toDisplay.bodyCount:0000}";
            
            var ts = TimeSpan.FromSeconds(toDisplay.time);
            timeText.text = $"{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
    }
}