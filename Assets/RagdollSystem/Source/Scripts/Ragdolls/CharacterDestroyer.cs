using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RagdollSystem;

namespace Unit.Puppet
{
    public class CharacterDestroyer : MonoBehaviour
    {
        [SerializeField] private float _delay = 5f;
        [SerializeField] private float _fadeOutDuration = 2f;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private List<SpriteRenderer> _spriteRenderersBody;
        [SerializeField] private List<SpriteRenderer> _spriteRenderersBone;

        private void Start() => _healthComponent.OnDeath += OnDeath;
        private void OnDestroy() => _healthComponent.OnDeath -= OnDeath;

        private void OnDeath()
        {
            _healthComponent.OnDeath -= OnDeath;

            StartCoroutine(DelaydDestroy());
            IEnumerator DelaydDestroy()
            {
                yield return new WaitForSeconds(_delay);
                gameObject.SetActive(false);
            }
        }
    }
}
