using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;

namespace RagdollSystem
{
    public enum GravityState
    {
        NormalGravity = 0,
        ZeroGravity = 1,
    }

    public class ActivePuppetState : State
    {
        private const float JumpGraviry = -10f;
        private const float JumpUpGravity = 10f;
        private const float Gravity = -10f;
        private const float UpGravity = 10;
        private const float Smooth = 0.3f;
        private const CharacterBoneType Component = CharacterBoneType.BodyUp;

        private readonly IReadOnlyCollection<Muscle2D> _muscles;
        private readonly ICoroutine _coroutine;
        private readonly StandingSystem _standingSystem;
        private readonly PuppetMaster2D _puppetMaster2D;
        private Vector3 _offset = Vector3.zero;
        private Vector3 _moveVector;
        private Vector3 _jumpVector;
        private float _yForce;
        private float _yForceUp;
        private bool _overrideGravity;
        private bool _isForced;
        private float _upGravity;
        private float _downGravity;
        private bool _test;
        private Transform _testTarget;
        private Transform _animationComponent;
        private GravityState _gravityState = GravityState.NormalGravity;

        public event Action OnLanding;

        public ActivePuppetState(PuppetMaster2D puppetMaster2D, Muscle2D[] muscles, StandingSystem standingSystem, ICoroutine coroutine)
        {
            _standingSystem = standingSystem;
            _puppetMaster2D = puppetMaster2D;
            _coroutine = coroutine;
            _muscles = muscles;
        }

        private void NormilizeGravity()
        {
            _upGravity = UpGravity;
            _downGravity = Gravity;
        }

        private void JumpGravity()
        {
            _upGravity = JumpUpGravity;
            _downGravity = JumpGraviry;
        }

        public override void Enter()
        {
            EnableOverrideGravity();
            NormilizeGravity();
        }

        public void EnableOverrideGravity() => _overrideGravity = true;
        public void DisableOverrideGravity() => _overrideGravity = false;

        public void Jump(Vector3 force, float duration, float delay)
        {
            JumpGravity();
            if (delay > 0)
            {
                _coroutine.StartCoroutine(Waiting());
                IEnumerator Waiting()
                {
                    yield return new WaitForSeconds(delay);
                    Execute();
                }
            }
            else
            {
                Execute();
            }

            void Execute()
            {
                _isForced = true;
                var forceUp = force * _upGravity;
                var forceDown = new Vector3(forceUp.x, 0, forceUp.z);

                DOTween.To(x => _jumpVector = Vector3.Slerp(Vector3.zero, forceUp, x), 0.65f, 1, duration / 2).OnComplete(() =>
                {
                    DOTween.To(x => _jumpVector = Vector3.Slerp(forceUp, forceDown, x), 0f, 1, duration / 2).timeScale = Time.timeScale;
                }).timeScale = Time.timeScale;

                DOVirtual.DelayedCall(duration, () =>
                {
                    _isForced = false;
                    NormilizeGravity();

                    OnLanding?.Invoke();
                }).timeScale = Time.timeScale;
            }
        }

        public override void PhysicsUpdate()
        {
            if (_test)
                _animationComponent.rotation = _testTarget.rotation;

            switch (_gravityState)
            {
                case GravityState.NormalGravity:
                    NormalGravityUpdate();
                    break;
                case GravityState.ZeroGravity:
                    ZeroGravityUpdate();
                    break;
            }
        }

        private void ZeroGravityUpdate()
        {
            _offset = _puppetMaster2D.Offset;
            _moveVector = new Vector3(_puppetMaster2D.HorizontalInput, _standingSystem.Height < _puppetMaster2D.DistanceToUp ? GetSmoothVerticalInput() : _puppetMaster2D.VerticalInput, 0);

            foreach (var muscle in _muscles)
                muscle.Update(_puppetMaster2D.PinWeight * _puppetMaster2D.MaxPin, _puppetMaster2D.RotationPinWeight, _moveVector, _offset);
        }

        private float GetSmoothVerticalInput()
        {
            var percent = (_puppetMaster2D.DistanceToUp - _standingSystem.Height) / Smooth;
            return Mathf.Lerp(_puppetMaster2D.VerticalInput, 0, percent);
        }

        private void NormalGravityUpdate()
        {
            _offset = _puppetMaster2D.Offset;

            if (_isForced)
                _moveVector = new Vector3(_puppetMaster2D.HorizontalInput, 0, 0) + _jumpVector;
            else
                _moveVector = new Vector3(_puppetMaster2D.HorizontalInput, 0, 0);

            if (_overrideGravity)
            {
                if (_standingSystem.Height > _puppetMaster2D.DistanceFromGround)
                    _yForce = _downGravity + _downGravity * _standingSystem.GravityFactor;
                else
                    _yForce = 0;

                foreach (var muscle in _muscles)
                {
                    if (muscle.CharacterBoneType == Component)
                    {
                        if (_standingSystem.Height < _puppetMaster2D.DistanceToUp)
                            _yForceUp = _upGravity;
                        else
                            _yForceUp = 0;

                        muscle.Update(_puppetMaster2D.PinWeight * _puppetMaster2D.MaxPin, _puppetMaster2D.RotationPinWeight, _moveVector + Vector3.up * _yForceUp, _offset);
                    }
                    else
                    {
                        muscle.Update(_puppetMaster2D.PinWeight * _puppetMaster2D.MaxPin, _puppetMaster2D.RotationPinWeight, _moveVector + Vector3.up * _yForce, _offset);
                    }
                }
            }
            else
            {
                foreach (var muscle in _muscles)
                    muscle.Update(_puppetMaster2D.PinWeight * _puppetMaster2D.MaxPin, _puppetMaster2D.RotationPinWeight, _moveVector, _offset);
            }
        }

        public override void Exit() => DisableOverrideGravity();
        public void ZeroGravity() => _gravityState = GravityState.ZeroGravity;

        public void NormalGravity()
        {
            _gravityState = GravityState.NormalGravity;

            foreach (var muscle in _muscles)
                muscle.ResetVelocity();
        }

        public void AnchorAnimationComponent(Transform point, Transform animationComponentTransform)
        {
            _test = true;
            _testTarget = point;
            _animationComponent = animationComponentTransform;
        }

        public void RemoveAnchorFromAnimation()
        {
            _test = false;
            _animationComponent.transform.rotation = Quaternion.identity;
        }
    }
}
