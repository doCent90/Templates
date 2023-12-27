using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Core;

namespace PhysicleMaterialsDamager
{
    public abstract class PickableWeapon : BaseWeapon, IDragAndDrop, IPushable
    {
        [SerializeField] private bool _disableCollisionOnPickUp;
        [SerializeField] private List<Collider2D> _colliders;
        [SerializeField] private PickableWeaponData _defaultData;
        [SerializeField] private List<GameObject> _changeLayerToDamage;
        [SerializeField] private List<Collider2D> _disableCollisionWithOwner;
        private readonly DragAndDropAttribute _dragAndDropAttribute = DragAndDropAttribute.None;

        [SerializeField] private UnityEvent _onDrag;
        [SerializeField] private UnityEvent _onDrop;

        [SerializeField] protected Rigidbody2D Rigidbody;
        [SerializeField] protected float Damping = 15f;
        [SerializeField] protected float Linear = 5f;

        protected WeaponDropState DropState;
        protected WeaponPickUpState PickUpState;
        protected bool IsPicked;

        public PickableWeaponData Data { get; private set; }
        public List<Collider2D> Colliders => _colliders;
        public Rigidbody2D SelfRigidbody => Rigidbody;

        private void OnCollisionEnter2D(Collision2D collision) => OnTouch(collision);
        protected virtual void Update() => StateMachine?.CurrentState?.Update();
        protected void FixedUpdate() => StateMachine?.CurrentState?.PhysicsUpdate();

        public void Init()
        {
            Data = _defaultData;
            DropState = new WeaponDropState();
            PickUpState = new WeaponPickUpState();
            StateMachine.ChangeState(DropState);
        }

        public void DefaultRotate() => Rotate(_objectRotator.DefaultLookSide);

        public void Rotate(LookSide lookSide)
        {
            if (IsPicked)
            {
                _objectRotator.SetupRotation(lookSide);
                RotateVFX();
            }
            else
            {
                if (_objectRotator.Rotate(lookSide))
                    RotateVFX();
            }
        }

        public void EnablePhysics() => Rigidbody.bodyType = RigidbodyType2D.Dynamic;
        public void DisablePhysics() => Rigidbody.bodyType = RigidbodyType2D.Kinematic;

        protected virtual void OnTouch(Collision2D collision) { }
        #region Interfaces
        protected virtual bool TryBeginDrag()
        {
            StateMachine.ChangeState(new MotionState(Rigidbody, CameraConverter, (Damping, Linear)));
            _onDrag?.Invoke();
            return true;
        }

        protected virtual void Drop()
        {
            StateMachine.ChangeState(DropState);
            _onDrop?.Invoke();
        }

        protected void Push(Rigidbody2D rigidbody2D, Vector2 direction, float force) 
            => rigidbody2D.AddForce(direction * force, ForceMode2D.Impulse);

        DragAndDropAttribute IDragAndDrop.DragAndDropAttribute => _dragAndDropAttribute;
        Transform IDragAndDrop.Transform => transform;
        void IDragAndDrop.Click() { }
        bool IDragAndDrop.TryBeginDrag(Vector3 clickPoint) => TryBeginDrag();
        bool IDragAndDrop.TryDrag() => true;
        void IDragAndDrop.Drop() => Drop();
        void IPushable.Push(Rigidbody2D rigidbody2D, Vector2 direction, float force) => Push(rigidbody2D, direction, force);
        #endregion
        #region Components
#if UNITY_EDITOR
        private void OnValidate() => SetComponents();

        [ContextMenu(nameof(SetComponents))]
        private void SetComponents()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }
#endif
        #endregion
    }
}
