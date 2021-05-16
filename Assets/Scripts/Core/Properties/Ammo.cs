using UnityEngine;
namespace Guinea.Core
{
    [System.Serializable]
    public class Ammo
    {
        [SerializeField]
        public ObjectType type;
        [SerializeField]
        public int maxAmmo;
    }
}