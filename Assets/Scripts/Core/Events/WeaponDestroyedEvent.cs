using System;

namespace Guinea.Event
{
    public interface WeaponDestroyedEvent
    {
        event Action OnWeaponDestroyed;
    }

}