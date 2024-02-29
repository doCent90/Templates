using Extensions;
using System;
using System.Collections;
using UnityEngine;

namespace GameRecorder
{
    [Serializable]
    public class TicksHandler : ITicksHandler
    {
        [field: SerializeField] public float TickRate { get; private set; } = 0.2f;

        private ICoroutine _coroutine;

        public ulong Ticks { get; private set; }

        public event Action<ulong> OnTick;

        public void Construct(ICoroutine coroutine)
        {
            _coroutine = coroutine;

            _coroutine.StartCoroutine(Ticking());
        }

        private IEnumerator Ticking()
        {
            var wait = new WaitForSeconds(TickRate);

            while (true)
            {
                Ticks++;
                OnTick?.Invoke(Ticks);
                yield return wait;
            }
        }
    }
}
