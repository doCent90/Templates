using System;

namespace GameRecorder
{
    public interface ITicksHandler
    {
        ulong Ticks { get; }
        float TickRate { get; }

        event Action<ulong> OnTick;
    }
}