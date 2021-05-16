using UnityEngine;

namespace Guinea.Core
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
    [System.Serializable]
    public class Ammunition : ScriptableObject
    {

        [SerializeField]
        private int damage;
        [SerializeField]
        private float range;
        [SerializeField]
        private int maxAmmo;
        [SerializeField]
        private float fireRate;
        [SerializeField]
        private bool allowHolding;
        // [SerializeField]
        // private ObjectType muzzleFlash;
        [SerializeField]
        private ObjectType impactEffect;
        [SerializeField]
        private ItemInfo info;
        public int Damage { get { return damage; } }
        public float Range { get { return range; } }
        public int MaxAmmo { get { return maxAmmo; } }
        public float FireRate { get { return fireRate; } }
        public bool AllowHolding { get { return allowHolding; } }
        // public ObjectType MuzzleFlash { get { return muzzleFlash; } }
        public ObjectType ImpactEffect { get { return impactEffect; } }
        public ItemInfo Info { get => info; }
        void OnValidate()
        {
            // muzzleFlash = Utility.Validate(muzzleFlash, ObjectType.MUZZLE_FLASH_00, ObjectType.MUZZLE_FLASH_COUNT);
            impactEffect = Utility.Validate(impactEffect, ObjectType.IMPACT_EFFECT_00, ObjectType.IMPACT_EFFECT_COUNT);
        }
    }
}