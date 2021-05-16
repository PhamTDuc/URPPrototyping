using UnityEngine;
using System.Collections.Generic;
namespace Guinea.Core
{
    [System.Serializable]
    public class PlayableProperties : MonoBehaviour, ICanInteract, IShootable
    {
        [Header("Weapon and Inventory")]
        [SerializeField]
        private Stats stats;
        [SerializeField]
        private List<ObjectType> weapons;
        private Dictionary<ObjectType, int> currentAmmos = new Dictionary<ObjectType, int>();
        [SerializeField]
        private int current_index;
        public int CurrentIndex => current_index;
        public ObjectType Weapon => weapons.Count > 0 ? weapons[current_index] : ObjectType.NONE;
        public int WeaponCount => weapons.Count;
        [SerializeField]
        private ObjectType grenade;
        public ObjectType Grenade { get { return grenade; } }
        [Header("Vehicle")]
        [SerializeField]
        private float motor;
        public float Motor { get { return motor; } }
        [SerializeField]
        private float steer_angle;
        public float SteerAngle { get { return steer_angle; } }
        [SerializeField]
        private float brake_force;
        public float BrakeForce { get { return brake_force; } }
        [SerializeField]
        private float ratio;
        public float Ratio { get { return ratio; } }
        [Header("Components")]
        [SerializeField]
        private GameObject visualWheel;
        public GameObject VisualWheel { get { return visualWheel; } }
        [Header("Sight of View")]
        [SerializeField]
        private float viewRadius;
        public float ViewRadius { get { return viewRadius; } }
        [SerializeField]
        [Range(0.0f, 360.0f)]
        private float viewAngle;
        public float ViewAngle { get { return viewAngle; } }
        [Header("Memory")]
        [SerializeField]
        private float memorySpan;
        public float MemorySpan { get { return memorySpan; } }
        [SerializeField]
        private float delay;
        public float MemoryDelay { get { return delay; } }
        [Header("States Transition Attributes")]
        [SerializeField]
        private float attackRange;
        public float AttackRange { get { return attackRange; } }
        [SerializeField]
        private string opponentLayerName;
        public string OpponentLayerName { get { return opponentLayerName; } }
        [SerializeField]
        private float attackAngle;
        public float AttackAngle { get { return attackAngle; } }
        [Header("Debug")]
        [SerializeField]
        private bool debug;
        void Awake()
        {
            Commons.Assert(stats != null, "PlayableProperties.cs: Stats can't not be null");
            stats = Instantiate(stats); // Get Default Scriptable Object
        }
        void Start()
        {
            foreach (var weapon in weapons)
            {
                currentAmmos.Add(weapon, MasterManager.GetLoaderManager().GetMaxAmmo(weapon));
            }
        }

        public bool EquipWeapon(ObjectType type)
        {
            if (!weapons.Exists(weapon => weapon == type))
            {
                weapons.Add(type);
                currentAmmos.Add(type, MasterManager.GetLoaderManager().GetMaxAmmo(type));
                return true;
            }
            return false;
        }

        public int WeaponsCount { get { return weapons.Count; } }

        public bool UnequipWeapon(ObjectType type)
        {
            if (weapons.Exists(weapon => weapon == type))
            {
                weapons.RemoveAll(weapon => weapon == type);
                currentAmmos.Remove(type);// TODO: Remove from currentAmmos
                current_index = weapons.Count == 0 ? -1 : weapons.Count - 1;
                return true;
            }
            return false;
        }

        public bool Use(ObjectType type)
        {
            if (isAvailable(type))
            {
                stats[type].quantity--;
                return true;
            }
            return false;
        }

        public bool isAvailable(ObjectType type)
        {
            return stats.Exists(type) && stats[type].quantity > 0;
        }
        public void AddStats(Stats new_stats)
        {
            foreach (Stat stat in new_stats)
            {
                stats.Add(stat);
            }
        }

        public Stat GetStat(ObjectType type)
        {
            return stats[type];
        }

        public bool StatExists(ObjectType type)
        {
            return stats.Exists(type);
        }

        public bool ChangeWeapon(int index)
        {
            if (weapons.Count == 0) return false;
            int count = weapons.Count;
            // Commons.Assert(count > 0, "PlayableProperties.cs ChangeWeapon: Weapons equipped could not be empty");
            index = index < 0 ? index + count : index;
            index %= count;
            if (current_index != index)
            {
                current_index = index;
                return true;
            }
            return false;
        }

        public int GetCurrentAmmo() => weapons.Count > 0 ? currentAmmos[weapons[current_index]] : -1;
        public int GetMaxAmmo() => weapons.Count > 0 ? MasterManager.GetLoaderManager().GetMaxAmmo(weapons[current_index]) : -1;

        public void SetGrenade(ObjectType new_grenade) => grenade = new_grenade;

        private void OnValidate()
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i] = Utility.Validate(weapons[i], ObjectType.CANNON_00, ObjectType.CANNON_COUNT);
            }
            grenade = Utility.Validate(grenade, ObjectType.GRENADE_00, ObjectType.GRENADE_COUNT);
        }

        public bool Shoot(bool pressed)
        {
            if (current_index == -1) return false;
            if (pressed && currentAmmos[weapons[current_index]] > 0)
            {
                currentAmmos[weapons[current_index]]--;
                return true;
            }
            return false;
        }

        public bool Reload()
        {
            if (current_index == -1) return false;
            if (Use(weapons[current_index]))
            {
                currentAmmos[weapons[current_index]] = MasterManager.GetLoaderManager().GetMaxAmmo(weapons[current_index]);
                if (debug) Commons.Log($"Reloading {weapons[current_index]}. Remaining: {GetStat(weapons[current_index]).quantity}");
                return true;
            }

            if (debug) Commons.Log($"Weapon {weapons[current_index]} is Empty ^^");
            return false;
        }
        private void OnDrawGizmosSelected()
        {
            #region Attack Range and Angle
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Vector3 right = transform.forward * attackRange;
            right = Quaternion.AngleAxis(attackAngle, transform.up) * right;
            Vector3 left = Quaternion.AngleAxis(-attackAngle * 2.0f, transform.up) * right;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, transform.position + left);
            Gizmos.DrawLine(transform.position, transform.position + right);
            #endregion

            #region Sight Of View
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            Vector3 leftView = DirectionFromAngle(transform.eulerAngles.y, -viewAngle / 2.0f);
            Vector3 rightView = DirectionFromAngle(transform.eulerAngles.y, viewAngle / 2.0f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + leftView * viewRadius);
            Gizmos.DrawLine(transform.position, transform.position + rightView * viewRadius);
            #endregion

            #region Helpers
            Vector3 DirectionFromAngle(float eulerY, float deg)
            {
                deg += eulerY;
                return new Vector3(Mathf.Sin(deg * Mathf.Deg2Rad), 0.0f, Mathf.Cos(deg * Mathf.Deg2Rad));
            }
            #endregion
        }
    }
}