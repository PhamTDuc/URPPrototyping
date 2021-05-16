using System.Collections;
using UnityEngine;
namespace Guinea.Core
{
    public class Weapon : PoolObjectBase, IShootable
    {
        [SerializeField]
        private Ammunition ammo;
        private float timer;
        private bool isReloading;
        private bool canShot = true;
        [SerializeField]
        private ParticleSystem muzzle;
        [SerializeField]
        private LayerMask layer;
        [Header("Debug")]
        [SerializeField]
        private bool debug;

        public Ammunition Ammo { get => ammo; }
        void OnEnable()
        {
            timer = 0.0f;
            isReloading = false;
            canShot = true;
        }

        public bool Shoot(bool pressed)
        {
            if (!ammo.AllowHolding && !pressed)
            {
                canShot = true;
            }

            if (pressed && !isReloading && Time.time >= timer)
            {
                if (canShot)
                {
                    timer = Time.time + 1.0f / ammo.FireRate;
                    ShootInternal();
                    if (!ammo.AllowHolding) canShot = false;
                    return true;
                }
            }
            return false;
        }

        private void ShootInternal()
        {
            // Debug.DrawRay(local.position + local.forward * 5, local.forward * 40, Color.green, .5f, true);
            if (muzzle != null) muzzle.Play();
            RaycastHit hit;
            if (Physics.Raycast(transform.position + transform.forward * 4f, transform.forward, out hit, ammo.Range, layer))
            {
                if (debug) Commons.Log($"Hit {hit.collider.name}");
                // if (impactEffect != null) Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); // TODO: Using PoolManager instead of Instantiate
                MasterManager.GetPoolManager().Spawn(ammo.ImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                IDestructible destructible = hit.collider.GetComponent<IDestructible>();
                destructible?.GetDamage(ammo.Damage);
            }
        }

        public bool Reload()
        {
            StopAllCoroutines();
            StartCoroutine(ReloadInternal());
            return true;
        }

        IEnumerator ReloadInternal()
        {
            isReloading = true;
            yield return new WaitForSeconds(1.0f);
            isReloading = false;
        }
        void OnValidate()
        {
            objectType = Utility.Validate(objectType, ObjectType.CANNON_00, ObjectType.CANNON_COUNT);
        }

    }
}