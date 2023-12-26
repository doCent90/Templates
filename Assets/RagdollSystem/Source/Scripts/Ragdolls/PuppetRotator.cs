using System;
using System.Collections.Generic;
using UnityEngine;
using Puppet2D;

namespace RagdollSystem
{
    public enum LookSide
    {
        Right = 1,
        Left = -1,
    }

    public class PuppetRotator : MonoBehaviour
    {
        [SerializeField] private List<Puppet2D_IKHandle> _flipHandle;
        [SerializeField] private Transform _animationComponent;
        [SerializeField] private Transform _physicsComponent;
        [SerializeField] private Transform _mainBody;
        [SerializeField] private List<Transform> _physicPart;
        [SerializeField] private Character _character;
        [SerializeField] private JointLimits _rightLimits;
        [SerializeField] private JointLimits _leftLimits;
        [SerializeField] private JointLimits _zeroLimits;
        [SerializeField] private LookSide _defaultSide;

        public LookSide LookSide { get; private set; }

        public event Action OnRotate;

        public void Init()
        {
            if (_defaultSide == LookSide.Left)
                Rotate(_defaultSide);

            LookSide = _defaultSide;
            _character.InitDefaultBoneLimits(_zeroLimits);
            OnRotate?.Invoke();
        }

        public void CheckDirection(float direction)
        {
            if (direction == 0)
                return;

            Rotate((LookSide)(int)Mathf.Sign(direction));
        }

        public void Rotate(LookSide lookSide)
        {
            if (LookSide == lookSide)
                return;

            LookSide = lookSide;
            ResetPhysicPosition();
            FlipScale(_animationComponent);
            FlipScale(_physicsComponent);
            FlipHandles();
            OnRotate?.Invoke();
        }

        public void SetLimits()
        {
            switch (LookSide)
            {
                case LookSide.Right: _character.SetBoneLimits(_rightLimits); break;
                case LookSide.Left: _character.SetBoneLimits(_leftLimits); break;
            }
        }

        public void RemoveLimits() => _character.SetBoneLimits(_zeroLimits);

        private void ResetPhysicPosition()
        {
            SetPartParent(transform);
            _physicsComponent.transform.position = _mainBody.transform.position;
            SetPartParent(_physicsComponent);
        }

        private void SetPartParent(Transform parent)
        {
            foreach (Transform part in _physicPart)
                part.SetParent(parent);
        }

        private void FlipScale(Transform transform)
            => transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        private void FlipHandles()
        {
            foreach (Puppet2D_IKHandle flipHandle in _flipHandle)
                flipHandle.Flip = !flipHandle.Flip;
        }
    }
}