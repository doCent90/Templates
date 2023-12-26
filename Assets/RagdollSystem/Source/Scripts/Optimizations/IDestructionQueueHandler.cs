using System;

namespace RagdollSystem
{
    public interface IDestructionQueueHandler
    {
        void AddEvent(Action onReady);
    }
}
