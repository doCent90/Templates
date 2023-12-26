using System;
using UnityEngine;

namespace RagdollSystem
{
    [Serializable]
    public abstract class State
    {
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void PhysicsUpdate() { }
        public virtual void LateUpdate() { }
        public virtual void InputVelocity(Vector2 input) { }
    }

    [Serializable]
    public abstract class EquipmentState : State
    {
        public virtual void StartAttack() { }

        public virtual void StopAttack() { }

        public virtual void MakeOneAttack() { }
    }
}
