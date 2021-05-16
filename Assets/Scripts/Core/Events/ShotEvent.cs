using System;
using Guinea.Core;

namespace Guinea.Event
{
    public interface ShotEvent
    {
        // event Action<int> OnShot;
        // event Action<int> OnReload;
        event Action<int, int, ItemInfo> OnWeapon;
    }

}