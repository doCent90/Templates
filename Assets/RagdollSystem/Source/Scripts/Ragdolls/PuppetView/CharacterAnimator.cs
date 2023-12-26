using Animancer;
using UnityEngine;

namespace RagdollSystem
{
    public class CharacterAnimator : MonoBehaviour
    {
        private const float BaseFade = 0.25f;

        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private MixerTransition2D _walkTransition;
        [Header("Animations")]
        [SerializeField] private AnimationClip _jumpStart;
        [SerializeField] private AnimationClip _jumpFly;
        [SerializeField] private AnimationClip _jumpEnd;
        [SerializeField] private float _speedFactor = 1f;

        private MixerState<Vector2> _movementState;
        private float _sitDownParameter = 0f;
        private float _speedParameter;

        public CharacterAnimationState CurrentState { get; private set; }

        public void Construct() => _movementState = _walkTransition.CreateState();

        public void WalkAnimation(float sitDown, float speed)
        {
            _sitDownParameter = Mathf.Clamp(sitDown * 0.5F, -1, 0);
            _speedParameter = Mathf.Abs(speed);
            _movementState.Parameter = new Vector2(_sitDownParameter, _speedParameter);
        }

        public float SetAnimation(CharacterAnimationState animationState, float fade = 0)
        {
            float value = 0;
            fade = fade > 0 ? fade : BaseFade;
            _animancer.Animator.speed = Time.timeScale * _speedFactor;
            CurrentState = animationState;

            switch (animationState)
            {
                case CharacterAnimationState.Movement:
                    var walkState = _animancer.Play(_movementState, fade);
                    walkState.Time = 0;
                    value = walkState.Duration;
                    break;
                case CharacterAnimationState.JumpStart:
                    var jumpStartState = _animancer.Play(_jumpStart, fade);
                    jumpStartState.Time = 0;
                    jumpStartState.Events.Add(1, () => _animancer.Play(_jumpFly, fade));

                    value = jumpStartState.Duration;
                    break;
                case CharacterAnimationState.JumpEnd:
                    var jumpEndState = _animancer.Play(_jumpEnd, fade);
                    jumpEndState.Time = 0;
                    jumpEndState.Events.Add(1, () =>
                    {
                        SetAnimation(CharacterAnimationState.Movement, fade);
                    });
                    value = jumpEndState.Duration;
                    break;
            }

            return value;
        }

        public void OnSpeedChanged(float speed) => _speedFactor = speed;
    }
}