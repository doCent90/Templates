using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace RagdollSystem
{
    public enum StandingState
    {
        None = 0,
        Active = 1,
        Fall = 2,
        Doll = 3,
        Dead = 4,
    }

    public class StandingSystem : MonoBehaviour
    {
        private const float MaxBeforeSpawn = 100f;
        private const float MinVelocityToGetUp = 0.03f;
        private const float ChekGetUp = 0.55f;
        private const float Delay = 2f;

        [SerializeField] private Collider2D _interationCollider;
        [SerializeField] private PuppetMaster2D _puppetMaster2D;
        [SerializeField] private Rigidbody2D _mainRigidBody;
        [SerializeField] private Transform _point;
        [SerializeField] private LayerMask _layer;
        [SerializeField] private float _length;
        private readonly RaycastHit2D[] _hitCache = new RaycastHit2D[5];
        private StandingState _lastState;
        private Tween _tween;
        private float _lastCheck;
        private bool _heightUpdated;
        private float _gravityFactore = 0;
        private bool _firstGroundCollision = false;

        public event Action<StandingState> OnChangingState;
        public StandingState State { get; private set; }
        public float Height { get; private set; }
        public bool IsJump { get; private set; }
        public float GravityFactor => _gravityFactore;
        public bool OnGround { get; private set; }

        private bool _lockActiveState;

        private IEnumerator Start()
        {
            State = StandingState.Active;

            while (_firstGroundCollision == false)
            {
                Array.Clear(_hitCache, 0, _hitCache.Length);
                Physics2D.RaycastNonAlloc(_point.position, -_point.up, _hitCache, MaxBeforeSpawn, _layer);

                Height = float.MaxValue;
                OnGround = false;
                _heightUpdated = false;
                foreach (RaycastHit2D hit in _hitCache)
                {
                    if (hit.collider == null)
                        continue;

                    Height = _point.transform.position.y - hit.point.y;

                    if (Height < _length)
                        _firstGroundCollision = true;
                }

                yield return null;
            }
        }

        private void Update() => BalacneDoll();
        private void LateUpdate() => _point.rotation = Quaternion.identity;

        public void LockActiveState() => _lockActiveState = true;
        public void UnlockActiveState() => _lockActiveState = false;
        public void MakeDead() => SetState(StandingState.Dead);
        public void MakeDoll() => SetState(StandingState.Doll);

        public void Stun(float duration = Delay)
        {
            if (_length == 0) return;
            float orig = _length;

            StartCoroutine(Sequence());
            IEnumerator Sequence()
            {
                _interationCollider.enabled = false;
                yield return new WaitForFixedUpdate();
                _length = 0;
                yield return new WaitForSeconds(duration);
                _length = orig;
                yield return new WaitForSeconds(1f);
                _interationCollider.enabled = true;
            }
        }

        public void RestoreLastState() => SetState(_lastState);

        public void Jump(float delta, float duration)
        {
            IsJump = true;
            _tween?.Complete();
            _tween?.Kill();

            _gravityFactore = 0;
            _length += delta;
            _tween = DOVirtual.DelayedCall(duration, () =>
            {
                _length -= delta;
                IsJump = false;
            });
        }

        private void BalacneDoll()
        {
            if (State == StandingState.Dead || _firstGroundCollision == false) return;
            Array.Clear(_hitCache, 0, _hitCache.Length);
            Physics2D.RaycastNonAlloc(_point.position, -_point.up, _hitCache, _length, _layer);

            Height = float.MaxValue;
            OnGround = false;
            _heightUpdated = false;
            foreach (RaycastHit2D hit in _hitCache)
            {
                if (hit.collider == null)
                {
                    continue;
                }

                if (!_heightUpdated)
                {
                    Height = _point.transform.position.y - hit.point.y;
                    _heightUpdated = true;
                }
                OnGround = true;
            }

            switch (State)
            {
                case StandingState.Active:
                    if (!OnGround && !IsJump)
                    {
                        _gravityFactore = 0.3f;
                        if (_lockActiveState) return;
                        SetState(StandingState.Fall);
                    }
                    break;
                case StandingState.Fall:
                    if (IsJump) return;
                    if (Time.time + ChekGetUp >= _lastCheck)
                    {
                        if (_mainRigidBody.velocity.magnitude <= MinVelocityToGetUp && OnGround)
                        {
                            _gravityFactore = 0;
                            SetState(StandingState.Active);
                        }

                        _lastCheck = Time.time;
                    }
                    break;
            }
        }

        private void SetState(StandingState state)
        {
            if (State == state) 
                return;

            _lastState = State;
            State = state;

            switch (State)
            {
                case StandingState.None:
                    break;
                case StandingState.Fall:
                    _puppetMaster2D.Fall();
                    break;
                case StandingState.Active:
                    _puppetMaster2D.GetUp();
                    break;
                case StandingState.Doll:
                    _puppetMaster2D.Fall();
                    break;
                case StandingState.Dead:
                    _puppetMaster2D.Dead();
                    break;
            }

            OnChangingState?.Invoke(State);
        }
    }
}
