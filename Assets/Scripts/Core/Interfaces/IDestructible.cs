using System;

namespace Guinea.Core
{
    public interface IDestructible
    {
        float maxHealth { get; }
        float currentHealth { get; }
        void GetDamage(int damage);
    }
}