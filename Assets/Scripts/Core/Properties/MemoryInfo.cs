using UnityEngine;

namespace Guinea.Core
{
    public class MemoryInfo
    {
        public GameObject obj = null;
        // public Vector3 position;
        // public Vector3 direction;
        // public float angle;
        public float lastSeen;
        public float score;
        public float Age { get { return Time.time - lastSeen; } }
    }

}