using System;
using System.Collections;
using Core;
using DG.Tweening;
using Unit.Puppet;
using UnityEngine;

namespace RagdollSystem
{
    public class PuppetMaster2D : MonoBehaviour, ICoroutine
    {
        [SerializeField] private Muscle2D[] _muscles = new Muscle2D[0];
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _anchor;
        [SerializeField] private Rigidbody2D _mainBody;
        [SerializeField] private LimbRetainer _limbRetainer;
        [SerializeField] private PuppetRotator _puppetRotator;
        [SerializeField] private GameObject _physicsComponent;
        [SerializeField] private GameObject _animationComponent;
        [SerializeField] private StandingSystem _standingSystem;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _maxDownOffset;
        [SerializeField] private float _groundOffset;
        [SerializeField] private float _referenceTimestep;
        [Min(0)][SerializeField] private float _distanceFromGround;
        [Min(0)][SerializeField] private float _distanceToUp;
        [Space][Header("Fall")]
        [Range(0, 1)][SerializeField] private float _fallenPinWeight = 0f;
        [Range(0, 1)][SerializeField] private float _normalPinWeight = 1f;
        [Space][Header("Master Weights")]
        [SerializeField][Range(0f, 1f)] private float _pinWeight = 1f;
        [SerializeField][Range(0f, 10000f)] private float _rotationPinWeight = 1f;
        [SerializeField][Min(0)] private float _maxPin = 1f;

        private StateMachine _stateMachine;
        private ActivePuppetState _activeState;
        private FallPuppetState _fallState;
        private GetUpPuppetState _getUpState;
        private Tween _pinTween;
        private GravityState _gravityState;
        private float _distanceFromSave;
        private float _distanceToSave;

        public float HorizontalInput { get; set; }
        public float VerticalInput { get; set; }
        public float PinWeight => _pinWeight;
        public float RotationPinWeight => _rotationPinWeight;
        public float MaxPin => _maxPin;
        public float DistanceFromGround => _distanceFromGround;
        public float DistanceToUp => _distanceToUp;
        public Vector3 Offset => _anchor.position - _target.position + _offset;
        public GravityState GravityState => _gravityState;

        public event Action OnActiveState;
        public event Action OnLanding;

        private void Awake() => Init();
        private void Update() => _stateMachine.CurrentState.PhysicsUpdate();
        private void LateUpdate() => _stateMachine.CurrentState.LateUpdate();

        public void NormalGravity()
        {
            if (_gravityState == GravityState.NormalGravity) return;

            _activeState.NormalGravity();
            _gravityState = GravityState.NormalGravity;
            OnGravity();
        }

        private void Init()
        {
            _distanceFromSave = _distanceFromGround;
            _distanceToSave = _distanceToUp;

            _stateMachine = new StateMachine();
            _activeState = new ActivePuppetState(this, _muscles, _standingSystem, this);
            _activeState.OnLanding += OnJumpComplete;
            _fallState = new FallPuppetState();
            _getUpState = new GetUpPuppetState();

            foreach (var muscle in _muscles)
                muscle.Init();

            _stateMachine.ChangeState(_activeState);
        }

        private void OnJumpComplete() => OnLanding?.Invoke();

        public void SetDownOffset(float percent)
        {
            if (_gravityState == GravityState.ZeroGravity) 
                return;

            var delta = Mathf.Clamp(percent * 1.65f, -1, 0) * _maxDownOffset;
            var groundDelta = Mathf.Clamp(percent * 2f, -1, 0) * _groundOffset;
            _offset = new Vector3(_offset.x, delta, _offset.z);
            _distanceFromGround = _distanceFromSave - groundDelta;
            _distanceToUp = _distanceToSave - groundDelta;
        }

        public void Fall()
        {
            if (_gravityState == GravityState.ZeroGravity)
                return;

            if (_stateMachine.CurrentState == _fallState) 
                return;

            _stateMachine.ChangeState(_fallState);
            NormalGravity();
            SetPinWeight(_fallenPinWeight);
            _puppetRotator.SetLimits();
        }

        public void Dead()
        {
            Fall();
            _pinTween?.Kill();
        }

        public void GetUp()
        {
            if (_stateMachine.CurrentState == _getUpState)
                return;

            SetBehaviourState(_getUpState);
            _puppetRotator.RemoveLimits();
            SetPinWeight(_normalPinWeight);
            SetBehaviourState(_activeState);
        }

        private void SetBehaviourState(State state)
        {
            _stateMachine.ChangeState(state);

            if (state == _activeState)
                OnActiveState?.Invoke();
        }

        private void SetPinWeight(float value, float time = 0)
        {
            _pinTween?.Kill();

            if (time > 0)
                _pinTween = DOTween.To(x => _pinWeight = x, _pinWeight, value, time);
            else
                _pinWeight = value;
        }

        public void OnGravity()
        {
            foreach (var muscle in _muscles)
                muscle.OnGravityScale();
        }

        public void Jump(Vector3 force, float duration, float delay = 0)
        {
            if (_stateMachine.CurrentState == _activeState)
            {
                if (_gravityState == GravityState.ZeroGravity)
                    NormalGravity();

                _activeState.Jump(force, duration, delay);
            }
        }
    }
}
