using Core;
using System.Collections;
using UnityEngine;

namespace RagdollSystem
{
    public class PuppetJumpState : PuppetInputState
    {
        private const float TimeFactor = 1.8f;

        private Coroutine _jumpDelay;
        private Coroutine _jumpEnd;

        private readonly float _lengthDelta = 3f;
        private readonly float _jumpPower = 1.8f;
        private readonly float _jumpDuration = 0.5f;

        private readonly StandingSystem _standingSystem;
        private readonly CharacterAnimator _characterAnimator;
        private readonly PuppetInputState _nextState;

        private readonly ICoroutine _coroutine;

        public PuppetJumpState(BaseCharacter baseCharacter, StandingSystem standingSystem, PuppetMaster2D puppetMaster, CharacterAnimator characterAnimator,
            EqupmentStateMachine stateMachine, PuppetInputState nextState, float speed, ICoroutine coroutine)
            : base(baseCharacter, puppetMaster, stateMachine, speed)
        {
            _coroutine = coroutine;
            _standingSystem = standingSystem;
            _characterAnimator = characterAnimator;
            _nextState = nextState;
        }

        public override void Enter()
        {
            base.Enter();
            Jump();
        }

        public override void Exit()
        {
            base.Exit();

            if(_jumpDelay != null)
            {
                _coroutine.StopCoroutine(_jumpDelay);
                _jumpDelay = null;
            }

            if(_jumpEnd != null)
            {
                _coroutine.StopCoroutine(_jumpEnd);
                _jumpEnd = null;
            }
        }

        private void Jump()
        {
            var delay = 0;
            _characterAnimator.SetAnimation(CharacterAnimationState.JumpStart, 0.01f);
            _jumpDelay = _coroutine.StartCoroutine(PhysicsJumpWaiting());

            IEnumerator PhysicsJumpWaiting()
            {
                yield return new WaitForSeconds(delay);
                PhysicsJump();
            }

            _standingSystem.Jump(_lengthDelta, _jumpDuration * 2);
            _jumpEnd = _coroutine.StartCoroutine(LandingWaiting());

            IEnumerator LandingWaiting()
            {
                yield return new WaitForSeconds(_jumpDuration + delay * TimeFactor);
                Landing();
            }
        }

        private void PhysicsJump() => PuppetMaster.Jump(Vector3.up * _jumpPower, _jumpDuration);

        private void Landing()
        {
            _characterAnimator.SetAnimation(CharacterAnimationState.JumpEnd, fade: 0.15f);
            StateMachine.ChangeState(_nextState);
        }
    }
}
