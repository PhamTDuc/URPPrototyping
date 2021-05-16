using System;
using UnityEngine;
namespace Guinea.Core
{
    public interface IShootable
    {
        // event Action<int, int, Sprite> OnWeapon;

        bool Shoot(bool pressed);
        // void ChangeWeapon(int index);
        bool Reload();
    }
}