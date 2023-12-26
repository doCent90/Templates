using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Destrutible;

namespace RagdollSystem
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private List<DamageListener> _healthParts = new();
        [SerializeField] private List<HealthPart> _healthPartsComponents = new();
        [Min(1)][SerializeField] private int _maxHealth;

        public int MaxHealth => _maxHealth;
        public int Health { get; private set; }
        public bool IsAlive { get; private set; }
        public bool HasDamagedLegs { get; private set; }

        public event Action OnDeath;

        public void Construct(Character character, Stamp stamp)
        {
            _healthPartsComponents.ForEach(health => health.Construct(character, stamp));
            _healthParts.ForEach(damagePart => damagePart.Init(this));
            Health = _maxHealth;
            IsAlive = _maxHealth > 0;
        }

        public void ApplyDamage(int value) => ChangeValue(-value);
        public void DieImmediate() => ChangeValue(-_maxHealth);
        public void OnLegsDamaged() => HasDamagedLegs = true;

        private void ChangeValue(int delta)
        {
            if (Health == 0) 
                return;

            _healthPartsComponents.ForEach(part => part.OnDamaged());
            Health += delta;

            if (Health <= 0)
            {
                Health = 0;
                IsAlive = false;
                OnDeath?.Invoke();
            }
            else if (Health > _maxHealth)
            {
                Health = _maxHealth;
            }
        }
        #region Settings
#if UNITY_EDITOR
        [ContextMenu(nameof(SetComponents))]
        private void SetComponents()
        {
            _healthParts.Clear();
            _healthPartsComponents.Clear();

            _healthParts = GetComponentsInChildren<DamageListener>().ToList();
            _healthPartsComponents = GetComponentsInChildren<HealthPart>().ToList();
        }
#endif
        #endregion
    }
}
