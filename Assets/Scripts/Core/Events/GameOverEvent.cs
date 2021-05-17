using System;
using Guinea.Core;

namespace Guinea.Event
{
    public interface GameOverEvent
    {
        event Action<string> OnGameOverEvent;
        event Action<string> OnGameOverLose;
    }
}