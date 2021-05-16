using UnityEngine;
namespace Guinea.Core
{
    [System.Serializable]
    public class Bomb
    {
        [SerializeField]
        private ObjectType type;
        [SerializeField]
        private GameObject bomb;
        public ObjectType Type { get { return type; } }
        public GameObject Identity { get { return bomb; } }
    }
}