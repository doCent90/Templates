using UnityEngine;
using System.Collections;

namespace Core
{
    public interface ICoroutine
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
    }
}
