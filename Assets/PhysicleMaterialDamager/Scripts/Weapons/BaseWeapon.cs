using System.Collections.Generic;
using UnityEngine;
using Core;

namespace PhysicleMaterialsDamager
{
    public abstract class BaseWeapon : PhysicleEntity
    {
        [SerializeField] protected ObjectRotator _objectRotator;
        [SerializeField] protected List<VisualRotator> _visualFlips;

        protected CameraConverter CameraConverter;
        protected StateMachine StateMachine;

        public void Construct(CameraConverter cameraConverter)
        {
            base.Construct();
            StateMachine = new StateMachine();
            CameraConverter = cameraConverter;
            _objectRotator.Init();
        }

        protected void RotateVFX()
        {
            foreach (VisualRotator visualFlip in _visualFlips)
                visualFlip.Rotate();
        }

        public virtual void StartAttack() { }
        public virtual void StopAttack() { }
    }
}
