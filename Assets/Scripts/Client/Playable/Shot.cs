using UnityEngine;
using Guinea.Core;
using System;
using Guinea.Event;

namespace Guinea
{
    [RequireComponent(typeof(PlayableProperties))]
    public class Shot : MonoBehaviour, IShootable, ShotEvent
    {
        [SerializeField]
        private Transform local;
        [Header("Debug")]
        private bool debug;
        private Weapon weapon;
        private PlayableProperties properties;
        public event Action<int, int, ItemInfo> OnWeapon = delegate { };

        void Awake()
        {
            properties = GetComponent<PlayableProperties>();
        }

        void Start()
        {
            ChangeWeaponInternal(0);
            OnWeapon(properties.GetMaxAmmo(), properties.GetMaxAmmo(), weapon.Ammo.Info);
        }

        public bool Shoot(bool pressed)
        {
            if (weapon == null) return false;
            if (properties.GetCurrentAmmo() > 0 && weapon.Shoot(pressed))
            {
                properties.Shoot(pressed);
                OnWeapon(properties.GetCurrentAmmo(), properties.GetMaxAmmo(), weapon.Ammo.Info);
                return true;
            }
            return false;
        }

        public bool Reload()
        {
            if (weapon == null) return false;
            if (properties.Reload())
            {
                weapon.Reload();
                OnWeapon(properties.GetCurrentAmmo(), properties.GetMaxAmmo(), weapon.Ammo.Info);
                return true;
            }
            return false;
        }

        public void ChangeWeapon(int index)
        {
            if (ChangeWeaponInternal(index)) OnWeapon(properties.GetCurrentAmmo(), properties.GetMaxAmmo(), weapon.Ammo.Info);
        }

        private bool ChangeWeaponInternal(int index)
        {
            if (!properties.ChangeWeapon(index) && weapon != null || properties.WeaponsCount == 0) return false;

            WeaponDestroyedEvent weaponEvent;
            if (local.childCount > 0)
            {
                // Remove Event in old weapon
                weaponEvent = weapon.gameObject.GetComponent<WeaponDestroyedEvent>();
                if (weaponEvent != null) weaponEvent.OnWeaponDestroyed -= OnWeaponDestroyed;
                MasterManager.GetPoolManager().DeactiveOrDestroy(weapon.gameObject);
            }
            weapon = MasterManager.GetPoolManager().Spawn(properties.Weapon).GetComponent<Weapon>();
            weapon.transform.SetParent(local, false);
            weapon.gameObject.layer = local.gameObject.layer;

            // Dispatcher Event to new weapon
            weaponEvent = weapon.gameObject.GetComponent<WeaponDestroyedEvent>();
            if (weaponEvent != null) weaponEvent.OnWeaponDestroyed += OnWeaponDestroyed;
            return true;
        }
        private void OnWeaponDestroyed()
        {
            if (debug) Commons.Log("Calling OnWeaponDestroyed in Shot.cs");
            properties.UnequipWeapon(weapon.Type);
            if (properties.WeaponCount > 0)
            {
                ChangeWeapon(properties.WeaponCount - 1);
            }
            else
            {
                OnWeapon(0, 0, null);
            }
        }
    }
}