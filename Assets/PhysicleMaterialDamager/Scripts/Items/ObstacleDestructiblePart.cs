using UnityEngine;
using Core;

namespace PhysicleMaterialsDamager
{
    [RequireComponent(typeof(SpriteRenderer))]
    [DisallowMultipleComponent]
    public class ObstacleDestructiblePart : MonoBehaviour
    {
        private readonly float _fadeOutDuration = 0.5f;
        private float _fadeOutdelay = 4f;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        private float _delay;
        private float _startTime;

        private bool _isTimerOn;
        private bool _started = false;

        public void Init(float customDelay = 0f)
        {
            if (_started)
                return;

            if (customDelay > 0)
                _fadeOutdelay = customDelay;

            _delay = _fadeOutdelay;
            _startTime = Time.time;
            _isTimerOn = true;
            _started = true;
        }

        public void Update()
        {
            if (_isTimerOn == false)
                return;

            if (Time.time >= _startTime + _delay)
            {
                FadeOut();
                _isTimerOn = false;
            }
        }

        private void FadeOut()
            => _spriteRenderer.DOFadeSprite(0, _fadeOutDuration, () => Destroy(gameObject));
    }
}
