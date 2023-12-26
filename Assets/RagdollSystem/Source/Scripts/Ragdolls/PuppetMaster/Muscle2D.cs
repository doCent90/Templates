using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RagdollSystem
{
    [Serializable]
    public class Muscle2D
    {
        [Range(0, 10)] public float PinK = 1;
        public bool AlternativeValue;
        [Range(0, 10)] public float AlternativePinK = 1;
        [Range(0, 10)] public float VelocityPink = 1f;
        public CharacterBoneType CharacterBoneType;
        public Transform Target;
        private float _pinK;
        [FormerlySerializedAs("Rigidbody2D")] public Rigidbody2D RigidBody2D;
        [SerializeField] private bool _startRotation = false;

        private readonly float _gravityScale = 1f;

        public void Init()
        {
            RigidBody2D.gravityScale = _gravityScale;

            SetStartRotation();
            SetNormalPin();
        }

        public void SetStartRotation()
        {
            if (_startRotation)
                RigidBody2D.transform.rotation = Target.rotation;
        }

        public void Update(float pinWeightMaster, float rotationPin, Vector3 move = default, Vector3 offset = default)
        {
            Vector2 velocity = Target.transform.position - RigidBody2D.transform.position + move + offset;

            if (pinWeightMaster > 0)
                RigidBody2D.velocity = Vector2.Lerp(RigidBody2D.velocity, velocity, pinWeightMaster * VelocityPink);

            Quaternion moveToRotation = Quaternion.RotateTowards(RigidBody2D.transform.rotation, Target.rotation, rotationPin * _pinK);
            RigidBody2D.MoveRotation(moveToRotation);
        }

        public void SetNormalPin() => _pinK = PinK;
        public void ResetVelocity() => RigidBody2D.velocity = RigidBody2D.velocity.normalized;
        public void OnGravityScale() => RigidBody2D.gravityScale = _gravityScale;
    }
}
