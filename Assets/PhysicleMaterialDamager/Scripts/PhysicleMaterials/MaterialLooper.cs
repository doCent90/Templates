using System;
using System.Collections;
using UnityEngine;
using Core;

namespace PhysicleMaterialsDamager
{
    [Serializable]
    internal class MaterialLooper
    {
        private const float ActonTime = 0.5f;
        private const float RandomRange = 0.5f;

        private readonly IPhysicleMaterialsHandler _handler;
        private readonly ICoroutine _coroutine;
        private Coroutine _current;

        public MaterialLooper(IPhysicleMaterialsHandler handler, ICoroutine coroutine)
        {
            _handler = handler;
            _coroutine = coroutine;
        }

        public void Start(Collider2D collider, PhysicleMaterial material)
            => _current ??= _coroutine.StartCoroutine(Looping(collider, material));

        public void Stop()
        {
            if (_current == null) return;

            _coroutine.StopCoroutine(_current);
            _current = null;
        }

        private IEnumerator Looping(Collider2D collider, PhysicleMaterial material)
        {
            var wait = new WaitForSeconds(ActonTime);

            while (true)
            {
                if (material == null)
                {
                    Stop();
                    break;
                }

                yield return wait;
                _handler.OnCollided(collider, material, RandomRange);
            }
        }
    }
}
