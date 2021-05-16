using System;

namespace Guinea.Event
{
    public interface DestructibleEvent
    {
        event Action<float> OnDestructible;
        float maxHealth { get; }
        float currentHealth { get; }
    }
}