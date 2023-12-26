using UnityEngine;

namespace RagdollSystem
{
    public class PuppetMovementState : PuppetInputState
    {
        private readonly CharacterAnimator _characterAnimator;
        private readonly float _blockDownOffset = -0.5f;

        private float _speedFactor = 1f;

        public PuppetMovementState(BaseCharacter character, PuppetMaster2D puppetMaster, CharacterAnimator characterAnimator,
            EqupmentStateMachine equipmentStateMachine, float speed) : base(character, puppetMaster, equipmentStateMachine, speed)
            => _characterAnimator = characterAnimator;

        public override void Enter() => _characterAnimator.SetAnimation(CharacterAnimationState.Movement);
        public void OnSpeedFactorChanged(float speed) => _speedFactor = speed;

        public override void InputVelocity(Vector2 input)
        {
            var newInput = new Vector2(input.x * _speedFactor, input.y);
            base.InputVelocity(newInput);
            _characterAnimator.WalkAnimation(PuppetMaster.GravityState == GravityState.NormalGravity ? newInput.y > _blockDownOffset ? 0 : newInput.y : 0, newInput.x);

            if (input.y <= 0)
                PuppetMaster.SetDownOffset(newInput.y > _blockDownOffset ? 0 : newInput.y);
            else
                PuppetMaster.SetDownOffset(0);

            PuppetMaster.VerticalInput = newInput.y * _speed;
        }

        public override void Exit()
        {
            PuppetMaster.SetDownOffset(0);
            PuppetMaster.VerticalInput = 0;
            _characterAnimator.WalkAnimation(0, 0);
        }
    }
}
