using System;
using UnityEngine;

namespace PhysicleMaterialsDamager
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
}