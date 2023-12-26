using System;
using UnityEngine;
using Destructible2D;
using Destrutible;

namespace RagdollSystem
{
    public class HealthPart : DamageListener, IDamagable
    {
        [SerializeField] private bool _directDamage;
        [Header("Base")]
        [SerializeField] private DestructableData _destructableData;
        [SerializeField] private DestructionPartGenerator _partGenerator;
        [SerializeField] private Transform _jointedPart;
        [SerializeField] private BodyPartType _bodyPartType;
        [Header("Damage")]
        [Range(0, 1)][SerializeField] private float _maxPercentDamage;
        [SerializeField] private float _damageK;
        [SerializeField] private bool _critToHead = true;
        [SerializeField] private D2dDestructible _destructibleSkin;
        [SerializeField] private D2dDestructible _destructibleMeat;
        [SerializeField] private D2dFracturer _fracturerSkin;
        [SerializeField] private D2dFracturer _fracturerMeat;
        [Header("Joints")]
        [SerializeField] private bool _destroyJoint;
        [Range(0, 3)][SerializeField] private float _originalDamagePercentToBreak;
        [Range(1, 4)][SerializeField] private int _addedDamageToBreak = 3;
        [SerializeField] private Joint2D _joint2D;
        [Header("Feedback")]
        [SerializeField] private bool _spawnFountain;

        private Stamp _stamp;
        private Character _character;
        private DamageType _currentDamageType = DamageType.None;
        private int _maxDamage;
        private int _appliedDamage;
        private int _maxBreakDamage;
        private int _damageValue;
        private int _appliedJointDamage = 0;

        public DestructableData DestructableData => _destructableData;
        public event Action Damaged;

        private void OnDestroy()
        {
            if (_destructibleSkin != null)
                _destructibleSkin.OnModified -= OnSpriteModified;

            if (_destructibleMeat != null)
                _destructibleMeat.OnModified -= OnSpriteModified;
        }

        private void OnJointBreak2D(Joint2D brokenJoint) => HealthComponent.DieImmediate();

        public void Construct(Character character, Stamp stamp)
        {
            _stamp = stamp;
            _character = character;
        }

        public override void Init(HealthComponent healthComponent)
        {
            base.Init(healthComponent);

            if (_destructibleSkin != null)
                _destructibleSkin.OnModified += OnSpriteModified;

            if (_destructibleMeat != null)
                _destructibleMeat.OnModified += OnSpriteModified;

            _maxDamage = (int)(HealthComponent.MaxHealth * _maxPercentDamage);
            _maxBreakDamage = (int)(_maxDamage * _originalDamagePercentToBreak);
        }

        public void ApplyDamage(int value, bool force, DamageType type, Vector2 contact = default)
        {
            OnDamaged();
            _currentDamageType = type;

            if ((_destructibleSkin.AlphaRatio > 0 || _destructibleMeat.AlphaRatio > 0) && force == false)
                _damageValue = value;
            else
                ConfirmDamage(value, type);
        }

        public void CriticalDamage(bool crit)
        {
            if (_joint2D != null)
            {
                BreakJoint();
                if (crit)
                    OnDamaged();
            }
        }

        public void OnDamaged()
        {
            if (_partGenerator != null)
                _partGenerator.OnDamaged();
        }

        protected override void OnCollided(int value, Vector2 contact)
        {
            ApplyDamage(value, force: true, DamageType.Melee, contact);
            _stamp.Execute(DestructableData, contact);
        }

        private void OnSpriteModified(D2dRect rect)
        {
            if (_damageValue <= 0) return;

            ConfirmDamage(_damageValue, _currentDamageType);
            _damageValue = 0;
        }

        private void ConfirmDamage(int value, DamageType type)
        {
            if (_destroyJoint)
                ApplyDamageToJoint(value, type);

            if (HealthComponent.IsAlive)
            {
                Damaged?.Invoke();
                ApplyDamageToHealth(value);

                if (type == DamageType.Explosion && _bodyPartType == BodyPartType.Body && HealthComponent.IsAlive)
                    _character.DisableStandingSystemTemporary(duration: 2f);

                if (_bodyPartType == BodyPartType.Head && _critToHead)
                {
                    if (_joint2D != null)
                    {
                        BreakJoint();
                        HealthComponent.DieImmediate();
                        _partGenerator.DisableAll();
                        return;
                    }
                }

                OnDamaged();
            }
        }

        private void ApplyDamageToHealth(int value)
        {
            if (_directDamage)
            {
                HealthComponent.ApplyDamage(value);
                return;
            }

            var increasedValue = (int)(value * _damageK);

            if (_appliedDamage < _maxDamage)
            {
                if (_appliedDamage + increasedValue <= _maxDamage)
                {
                    _appliedDamage += increasedValue;
                }
                else
                {
                    increasedValue = _maxDamage - _appliedDamage;
                    _appliedDamage = _maxDamage;
                }
                HealthComponent.ApplyDamage(increasedValue);
            }
        }

        private void ApplyDamageToJoint(int value, DamageType type = DamageType.None)
        {
            if (_joint2D != null)
            {
                if (type == DamageType.Cold)
                    value *= _addedDamageToBreak;

                _appliedJointDamage += value;

                if (_appliedJointDamage >= _maxBreakDamage)
                {
                    BreakJoint();

                    if (type == DamageType.Cold)
                    {
                        _partGenerator.EnableAll();
                        _fracturerSkin.Fracture();
                        _fracturerMeat.Fracture();
                    }
                }
            }
        }

        private void BreakJoint()
        {
            OnJointBreak2D(_joint2D);
            _joint2D.connectedBody = null;
            Destroy(_joint2D);
            _joint2D = null;
        }
    }

    public enum BodyPartType
    {
        None = 0,
        Head = 1,
        Body = 2,
        Arms = 3,
        Legs = 4,
    }
}
