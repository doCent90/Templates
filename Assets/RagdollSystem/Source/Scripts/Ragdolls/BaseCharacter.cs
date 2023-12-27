using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace RagdollSystem
{
    public abstract class BaseCharacter : PhysicleEntity, ICharacter
    {
        [SerializeField] private CharacterType _characterType;
        [SerializeField] private List<CharacterBone> _bones;
        [SerializeField] private CharacterData _data;

        protected EqupmentStateMachine EquipmentStateMachine;
        protected float RotationAbs;

        public CharacterData Data => _data;
        public IReadOnlyList<CharacterBone> Bones => _bones;
        public IReadOnlyList<Collider2D> Colliders { get; protected set; }
        public CharacterType CharacterType => _characterType;
        public bool UnderPlayerControll { get; protected set; }

        public event Action<BaseCharacter> OnDie;

        protected virtual void Init()
        {
            EquipmentStateMachine = new EqupmentStateMachine();
            _bones = GetComponentsInChildren<CharacterBone>().ToList();
            foreach (CharacterBone bone in _bones)
                bone.Character = this;
        }

        public void InitDefaultBoneLimits(JointLimits boneLimits)
        {
            foreach (JointLimit limit in boneLimits.Limits)
            {
                CharacterBone currentBone = _bones.FirstOrDefault(item => item.Type == limit.Type);
                if (currentBone != null)
                    currentBone.InitDefaultLimits(limit);
            }
        }

        public void SetBoneLimits(JointLimits boneLimits)
        {
            foreach (JointLimit limit in boneLimits.Limits)
            {
                CharacterBone currentBone = _bones.FirstOrDefault(item => item.Type == limit.Type);
                if (currentBone != null)
                    currentBone.SetCustomLimit(limit);
            }
        }

        public virtual bool CanInteract() => false;
        public virtual void RotateInDirection(float value) { }
        public virtual void OnDead() => OnDie?.Invoke(this);
        public virtual bool OnTryBeginDrag(Rigidbody2D rigidbody) => false;
        public virtual void OnDrop() { }
    }

    public enum CharacterType
    {
        Puppet,
        Solid,
    }
}
