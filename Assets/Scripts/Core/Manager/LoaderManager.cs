using UnityEngine;
using System.Collections.Generic;

namespace Guinea.Core
{
    public class LoaderManager : MonoBehaviour, IManager
    {
        public ManagerStatus status { get; private set; }
        #region Weapons  And Bomb Loader 
        [SerializeField]
        private List<Ammo> ammos;
        private Dictionary<ObjectType, int> weapons_dict = new Dictionary<ObjectType, int>();
        #endregion

        void OnValidate()
        {
            foreach (var ammo in ammos)
            {
                ammo.type = Utility.Validate(ammo.type, ObjectType.CANNON_00, ObjectType.CANNON_COUNT);
            }
        }
        void Awake()
        {
            foreach (Ammo ammo in ammos)
            {
                weapons_dict.Add(ammo.type, ammo.maxAmmo);
            }
        }

        public void Initialize()
        {
            status = ManagerStatus.Initialized;
        }

        public int GetMaxAmmo(ObjectType weapon_id)
        {
            return weapons_dict[weapon_id];
        }
    }
}