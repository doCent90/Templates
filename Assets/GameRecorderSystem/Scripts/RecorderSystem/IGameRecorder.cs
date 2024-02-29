using System;
using System.Collections.Generic;

namespace GameRecorder
{
    public interface IGameRecorder
    {
        IReadOnlyList<PhysicleData[]> FramesData { get; }
        int CurrentFrameIndex { get; }

        void StopRecoding();
        void StartPlayMode(Action onStoped = null);
        void StopPlayMode(bool manualStop = false);
        void ManualPlay(int frameIndex);
        void ManualCancel();
    }
}