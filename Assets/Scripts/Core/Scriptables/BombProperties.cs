using UnityEngine;

namespace Guinea.Core
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "BombProperties", menuName = "Inventory/Bomb")]
    public class BombProperties : ScriptableObject
    {

        [SerializeField]
        private float delay;
        [SerializeField]
        private float radius;
        [SerializeField]
        private float force;
        // [SerializeField]
        // private GameObject effect;
        [SerializeField]
        private int damage;
        [SerializeField]
        private ObjectType effectType;

        public float Delay { get => delay; }
        public float Radius { get => radius; }
        public float Force { get => force; }
        public ObjectType Effect { get => effectType; }
        public int Damage { get => damage; }

        void OnValidate()
        {
            effectType = Utility.Validate(effectType, ObjectType.EXPLOSION_00, ObjectType.EXPLOSION_COUNT);
        }
    }
}