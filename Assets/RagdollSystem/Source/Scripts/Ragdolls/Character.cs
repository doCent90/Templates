using System.Collections.Generic;
using UnityEngine;
using Core;

namespace RagdollSystem
{
    public class Character : BaseCharacter, IInputListener, ICoroutine
    {
        [Header("Components")]
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private CharacterAnimator _characterAnimator;
        [SerializeField] private PuppetMaster2D _puppetMaster;
        [SerializeField] private StandingSystem _standingSystem;
        [SerializeField] private PuppetRotator _puppetRotator;
        [SerializeField] private HingeJoint2D _abdomenJoint;
        [Header("DragAndDrop")]
        [SerializeField] private float damping;
        [SerializeField] private float linear;

        private PuppetDeathState _deadState;
        private PuppetJumpState _jumpState;
        private CameraConverter _cameraConverter;
        private IControllInterlayer _controllInterlayer;

        public PuppetMovementState MovementState { get; private set; }
        public bool IsDead { get; private set; } = false;

        private void OnDestroy()
        {
            if (CharacterType == CharacterType.Puppet)
                _standingSystem.OnChangingState -= OnChangingStandState;

            foreach (var bone in Bones)
                bone.OnBroke -= OnDead;

            _healthComponent.OnDeath -= OnDead;
        }

        private void Update()
        {
            EquipmentStateMachine?.State?.Update();
            EquipmentStateMachine?.EquipmentState?.Update();
        }

        private void FixedUpdate() => EquipmentStateMachine?.State?.PhysicsUpdate();

        public void Construct(IControllInterlayer controllInterlayer, IReadOnlyList<Collider2D> colliders, IReadOnlyList<SpriteRenderer> spriteRenderers, IReadOnlyList<DestructionPartGenerator> destructionPartGenerators, CameraConverter cameraConverter)
        {
            _controllInterlayer = controllInterlayer;
            Colliders = colliders;
            _cameraConverter = cameraConverter;
            _characterAnimator.Construct();
            Init();
        }

        public override bool OnTryBeginDrag(Rigidbody2D rigidBody2D)
        {
            EquipmentStateMachine.ChangeState(new MotionPuppetState(_standingSystem, rigidBody2D, _cameraConverter, (damping, linear)));
            return true;
        }

        public override void OnDrop() => SetBehaviourState(MovementState);
        public override bool CanInteract() => UnderPlayerControll && EquipmentStateMachine.State == MovementState;

        public override void RotateInDirection(float value)
        {
            if (_standingSystem.State == StandingState.Active)
                _puppetRotator.CheckDirection(value);
        }

        public override void OnDead()
        {
            if (IsDead)
                return;

            IsDead = true;
            SetBehaviourState(_deadState);
            base.OnDead();
        }

        public void DisableStandingSystemTemporary(float duration)
        {
            if (UnderPlayerControll)
                _controllInterlayer.StopControll();

            _standingSystem.Stun(duration);
        }

        void IInputListener.SetUnderPlayerControll(bool value)
        {
            UnderPlayerControll = value;

            if (UnderPlayerControll)
                _standingSystem.LockActiveState();
            else
                _standingSystem.UnlockActiveState();
        }

        void IInputListener.LookAt(float lookSide)
        {
            if (Mathf.Abs(lookSide) < RotationAbs)
                return;

            RotateInDirection(lookSide);
        }

        void IInputListener.InputMovement(Vector2 direction) => EquipmentStateMachine.State?.InputVelocity(direction);
        void IInputListener.Jump() => TryJump();

        protected override void Init()
        {
            base.Init();
            _puppetRotator.Init();
            InitializeStateMachine();
            _standingSystem.OnChangingState += OnChangingStandState;

            foreach (CharacterBone bone in Bones)
                bone.OnBroke += OnDead;

            _healthComponent.OnDeath += OnDead;
        }

        private void OnChangingStandState(StandingState state)
        {
            if (UnderPlayerControll == false)
                return;

            switch (state)
            {
                case StandingState.Fall: Debug.Log("I`m Fall"); break;
                case StandingState.Dead: Debug.Log("I`m Dead"); break;
                case StandingState.Doll: Debug.Log("I`m Alive"); break;
            }
        }

        private void InitializeStateMachine()
        {
            MovementState = new PuppetMovementState(this, _puppetMaster, _characterAnimator, EquipmentStateMachine, Data.Speed);
            _deadState = new PuppetDeathState(_standingSystem);
            _jumpState = new PuppetJumpState(this, _standingSystem, _puppetMaster, _characterAnimator, EquipmentStateMachine, MovementState, Data.Speed, this);
            EquipmentStateMachine.InitializeControl(MovementState);
        }

        private void TryJump()
        {
            if (EquipmentStateMachine.State != MovementState || _standingSystem.State != StandingState.Active || _standingSystem.IsJump || !_standingSystem.OnGround) 
                return;

            EquipmentStateMachine.ChangeState(_jumpState);
        }

        private void SetBehaviourState(State state) => EquipmentStateMachine.ChangeState(state);
    }
}
