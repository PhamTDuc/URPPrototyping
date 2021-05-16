using Guinea.Core;
using Guinea.Event;
using System;

namespace Guinea.Destructible
{
    public class DestructibleWeapon : Destructible, WeaponDestroyedEvent
    {
        public event Action OnWeaponDestroyed = delegate { };

        protected override void DestroyWhenDied()
        {
            OnWeaponDestroyed();
            base.DestroyWhenDied();
        }
    }
}