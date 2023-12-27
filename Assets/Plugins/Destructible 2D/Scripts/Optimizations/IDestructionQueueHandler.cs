using System;

namespace Destructible2D
{
    public interface IDestructionQueueHandler
    {
        void AddEvent(Action onReady);
    }
}
