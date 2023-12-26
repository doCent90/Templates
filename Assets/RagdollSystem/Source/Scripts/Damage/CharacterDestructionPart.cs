using System.Collections;
using UnityEngine;

namespace RagdollSystem
{
    public class CharacterDestructionPart : MonoBehaviour
    {
        public void AutoDestroy(float delay)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutine(DelaydDestroy());
            IEnumerator DelaydDestroy()
            {
                var wait = new WaitForFixedUpdate();
                yield return new WaitForSeconds(delay);

                while(spriteRenderer.color.a > 0.1f)
                {
                    Color color = spriteRenderer.color;
                    color.a -= Time.fixedDeltaTime;
                    spriteRenderer.color = color;
                    yield return wait;
                }

                Destroy(gameObject);
            }
        }
    }
}
