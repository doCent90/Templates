using UnityEngine;

namespace RagdollSystem
{
    public class DamageListener : MonoBehaviour
    {
        private const float MinVelocityValue = 10f;
        private const float MinDamageValue = 1f;
        private const float MaxDamageValue = 3f;
        private const float Delay = 0.5f;

        private float _lastTime;

        public HealthComponent HealthComponent { get; protected set; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_lastTime + Delay > Time.time) return;

            if (collision.relativeVelocity.sqrMagnitude > MinVelocityValue)
            {
                _lastTime = Time.time;
                float value = Mathf.Clamp(collision.relativeVelocity.sqrMagnitude, MinDamageValue, MaxDamageValue);
                OnCollided((int)value, collision.contacts[0].point);
            }
        }

        public virtual void Init(HealthComponent healthComponent) => HealthComponent = healthComponent;

        protected virtual void OnCollided(int value, Vector2 contact) { }
    }
}
