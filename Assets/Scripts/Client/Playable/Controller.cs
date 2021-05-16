using UnityEngine;
using Guinea.Core;
using System;

namespace Guinea
{
    [RequireComponent(typeof(Throwable))]
    [RequireComponent(typeof(MoveController))]
    [RequireComponent(typeof(GunMovement))]
    [RequireComponent(typeof(Shot))]
    public class Controller : MonoBehaviour, IPlayable
    {
        protected MoveController moveController;
        protected GunMovement gunMovement;
        protected Shot shot;
        protected Throwable throwable;
        protected PlayableProperties properties;
        protected Rigidbody rb;

        public ObjectType Grenade => properties.Grenade;
        public int CurrentIndex => properties.CurrentIndex;

        public float Speed => rb.velocity.magnitude;
        public ObjectType CurrentWeapon => properties.Weapon;

        public GameObject Obj => gameObject;

        // public Transform Chasing { get; set; }

        public float AttackRange => properties.AttackRange;

        public string OpponentLayerName => properties.OpponentLayerName;

        public float AttackAngle => properties.AttackAngle;

        protected void Awake()
        {
            properties = GetComponent<PlayableProperties>();
            moveController = GetComponent<MoveController>();
            gunMovement = GetComponent<GunMovement>();
            shot = GetComponent<Shot>();
            throwable = GetComponent<Throwable>();
            rb = GetComponent<Rigidbody>();
        }
        public void Move(float forward, float steer, bool is_braked) => moveController.Move(forward, steer, is_braked);
        public void UpDown(float upward) => gunMovement.UpDown(upward);
        public void Shoot(bool pressed) => shot.Shoot(pressed);
        public void ChangeWeapon(int index) => shot.ChangeWeapon(index);
        public void Reload() => shot.Reload();
        public void SetGrenade(ObjectType grenade) => properties.SetGrenade(grenade);
        public void Throw(bool isPressed, Action action = null) => throwable.Throw(isPressed, action);
        public void SetThrowObject(ObjectType new_throwable) => throwable.SetThrowObject(new_throwable);
        public bool isAvailable(ObjectType stat) => properties.isAvailable(stat);
        public bool Use(ObjectType stat) => properties.Use(stat);
        public Stat GetStat(ObjectType stat) => properties.GetStat(stat);

        public void Reset()
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
        }

        public bool isTurnedOver() => Mathf.Abs(transform.localEulerAngles.z) > 60.0f;

        public int GetCurrentAmmo() => properties.GetCurrentAmmo();

    }
}