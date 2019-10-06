using System;
using UnityEngine;

namespace Stats
{
    public class TimesUpdater : MonoBehaviour
    {
        public Stats levelStats, gameStats;
        
        public void FixedUpdate()
        {
            levelStats.time += Time.fixedDeltaTime;
            gameStats.time += Time.fixedDeltaTime;
        }
    }
}