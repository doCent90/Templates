using Core;
using Destructible2D;
using UnityEngine;

namespace PhysicleMaterialsDamager
{
    public class EnvironmentObject : PhysicleEntity, IDragAndDrop, IPushable
    {
        private IdleState _idleState;
        private StateMachine _stateMachine;
        private CameraConverter _cameraConverter;

        [SerializeField] private float _damping;
        [SerializeField] private float _linear;
        [SerializeField] private bool _zeroVelocityAfterDrop;
        private readonly DragAndDropAttribute _dragAndDropAttribute = DragAndDropAttribute.None;

        [field: SerializeField] public Rigidbody2D RigidBody { get; protected set; }
        [field: SerializeField] public Entity ChildrenLogic { get; private set; }
        [field: SerializeField] public DestructibleObstacle DestructibleObstacle { get; private set; }

        private void Update() => _stateMachine?.CurrentState?.Update();
        private void FixedUpdate() => _stateMachine?.CurrentState?.PhysicsUpdate();

        public void Construct(CameraConverter cameraConverter, IDestructionQueueHandler destructionQueueHandler)
        {
            base.Construct();
            _cameraConverter = cameraConverter;

            if (ChildrenLogic != null)
            {
                PhysicleEntity lemonEntity = (PhysicleEntity)ChildrenLogic;
                lemonEntity.Construct();
            }

            if(DestructibleObstacle != null)
                DestructibleObstacle.Construct(cameraConverter, destructionQueueHandler, RigidBody);

            _stateMachine = new();
            _idleState = new();
            _stateMachine.Initialize(_idleState);
        }

        private bool TryBeginDrag(Vector3 clickPoint)
        {
            var state = new MotionState(RigidBody, _cameraConverter, (_damping, _linear));
            state.CreateClickPoint(clickPoint);
            _stateMachine.ChangeState(state);
            return true;
        }

        private void Drop()
        {
            _stateMachine.ChangeState(_idleState);

            if (_zeroVelocityAfterDrop)
                RigidBody.velocity = Vector2.zero;
        }

        private void Push(Rigidbody2D rigidbody2D, Vector2 direction, float force)
            => rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);

        DragAndDropAttribute IDragAndDrop.DragAndDropAttribute => _dragAndDropAttribute;

        Transform IDragAndDrop.Transform => transform;

        void IDragAndDrop.Click() { }
        bool IDragAndDrop.TryBeginDrag(Vector3 clickPoint) => TryBeginDrag(clickPoint);
        bool IDragAndDrop.TryDrag() => true;
        void IDragAndDrop.Drop() => Drop();
        void IPushable.Push(Rigidbody2D rigidbody2D, Vector2 direction, float force) => Push(rigidbody2D, direction, force);
        #region Settings
#if UNITY_EDITOR
        [ContextMenu(nameof(ResetSound))]
        private void ResetSound() => GetComponent<AudioSource>().volume = 0.6F;

        private void OnValidate() => SetComponents();

        [ContextMenu(nameof(SetComponents))]
        private void SetComponents()
        {
            DestructibleObstacle = GetComponent<DestructibleObstacle>();
        }
#endif
        #endregion
    }
}
