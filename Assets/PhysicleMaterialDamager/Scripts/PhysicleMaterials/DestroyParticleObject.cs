using System.Collections;
using UnityEngine;

namespace PhysicleMaterialsDamager
{
    public class DestroyParticleObject : MonoBehaviour
    {

        public float TimeToDestroy;

        private void Start()
        {
            StartCoroutine(DestroyThis());
        }

        private IEnumerator DestroyThis()
        {
            yield return new WaitForSeconds(TimeToDestroy);

            Destroy(gameObject);
        }
    }
}