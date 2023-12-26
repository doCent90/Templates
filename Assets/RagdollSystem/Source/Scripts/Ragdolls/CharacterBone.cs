using System;
using UnityEngine;

namespace RagdollSystem
{
    public class CharacterBone : MonoBehaviour, IDragAndDrop
    {
        [SerializeField] private HingeJoint2D _hingeJoint;
        [SerializeField] private FixedJoint2D _fixedJoint;
        private DragAndDropAttribute _dragAndDropAttribute = DragAndDropAttribute.None;
        private Vector2 _defaultLimit;
        private bool _isBroken;

        [field: SerializeField] public CharacterBoneType Type { get; private set; }
        [field: SerializeField] public Rigidbody2D RigidBody2D { get; private set; }
        public bool IsTouchCollision { get; private set; }
        public ICharacter Character { get; set; }

        public event Action OnBroke;

        private void OnJointBreak2D(Joint2D brokenJoint)
        {
            _isBroken = true;
            OnBroke?.Invoke();
        }

        private void OnCollisionStay2D(Collision2D collision) => IsTouchCollision = true;
        private void OnCollisionExit2D(Collision2D collision) => IsTouchCollision = false;

        public void InitDefaultLimits(JointLimit jointLimit)
        {
            SetCustomLimit(jointLimit);

            if (_hingeJoint != null)
            {
                _defaultLimit.x = _hingeJoint.limits.min;
                _defaultLimit.y = _hingeJoint.limits.max;
            }
        }

        public void SetCustomLimit(JointLimit jointLimit)
        {
            if (jointLimit.FixedJoint && _fixedJoint != null)
            {
                _hingeJoint.enabled = false;
                _fixedJoint.enabled = true;
            }
            SetLimit(jointLimit.Value, jointLimit.UseLimit);
        }

        private void SetLimit(Vector2 limit, bool useLimit)
        {
            if (_isBroken) return;
            var hingeLimit = _hingeJoint.limits;
            hingeLimit.min = limit.x;
            hingeLimit.max = limit.y;
            _hingeJoint.limits = hingeLimit;
            _hingeJoint.useLimits = useLimit;
        }

        DragAndDropAttribute IDragAndDrop.DragAndDropAttribute => _dragAndDropAttribute;
        Transform IDragAndDrop.Transform => transform;
        void IDragAndDrop.Click() { }
        bool IDragAndDrop.TryBeginDrag(Vector3 clickPoint) => Character.OnTryBeginDrag(RigidBody2D);
        bool IDragAndDrop.TryDrag() => true;
        void IDragAndDrop.Drop() => Character.OnDrop();
    }
}
