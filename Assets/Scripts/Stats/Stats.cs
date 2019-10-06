using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(fileName = "Stats", menuName = "Stats")]
    public class Stats : ScriptableObject
    {
        public int bodyCount = 0;
        public float time = 0;
    }
}
