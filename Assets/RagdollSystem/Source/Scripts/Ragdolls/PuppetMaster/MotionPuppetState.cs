using Core;
using UnityEngine;

namespace RagdollSystem
{
    public class MotionPuppetState : MotionState
    {
        private readonly StandingSystem _standingSystem;

        public MotionPuppetState(StandingSystem standingSystem, Rigidbody2D rigidbody, CameraConverter cameraConverter, (float, float) movingData)
            : base(rigidbody, cameraConverter, movingData)
            => _standingSystem = standingSystem;

        public override void Enter()
        {
            base.Enter();
            _standingSystem.MakeDoll();
        }

        public override void Exit()
        {
            base.Exit();
            _standingSystem.RestoreLastState();
        }
    }
}