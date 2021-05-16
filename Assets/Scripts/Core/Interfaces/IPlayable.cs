using System;
using UnityEngine;

namespace Guinea.Core
{
    public interface IPlayable
    {
        void Move(float forward = 0.0f, float steer = 0.0f, bool is_braked = false);
        void Reset();
        bool isTurnedOver();
        void UpDown(float upward);
        void Shoot(bool pressed);
        void ChangeWeapon(int index);
        void Reload();
        void SetGrenade(ObjectType grenade);
        void Throw(bool isPressed, Action action = null);
        void SetThrowObject(ObjectType new_throwable);
        bool isAvailable(ObjectType stat);
        bool Use(ObjectType stat);
        ObjectType Grenade { get; }
        ObjectType CurrentWeapon { get; }
        Stat GetStat(ObjectType stat);
        int CurrentIndex { get; }
        int GetCurrentAmmo();
        float Speed { get; }
        GameObject Obj { get; }
        // Transform Chasing { get; set; }
        float AttackRange { get; }
        float AttackAngle { get; }
        string OpponentLayerName { get; }
    }
}