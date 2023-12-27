using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Destructible2D
{
    public class DestructionQueueHandler : IDestructionQueueHandler
    {
        private const float Time = 0.15f;

        private readonly ICoroutine _coroutine;
        private readonly WaitForSecondsRealtime _wait;

        private Queue<Action> _destructables = new();
        private Coroutine _current;

        public DestructionQueueHandler(ICoroutine coroutine)
        {
            _coroutine = coroutine;
            _wait = new WaitForSecondsRealtime(Time);
        }

        public void AddEvent(Action onReady)
        {
            _destructables.Enqueue(onReady);

            if (_current == null)
                _current = _coroutine.StartCoroutine(Waiting());

            IEnumerator Waiting()
            {
                while (_destructables.Count > 0)
                {
                    for (int i = 0; i < _destructables.Count; i++)
                    {
                        Action onReady = _destructables.Dequeue();
                        onReady?.Invoke();
                        yield return _wait;
                    }
                }

                _current = null;
            }
        }
    }
}
