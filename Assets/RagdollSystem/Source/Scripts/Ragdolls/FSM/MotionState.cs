using System;
using UnityEngine;

namespace RagdollSystem
{
    [Serializable]
    public class MotionState : State
    {
        protected Rigidbody2D currentRigidBody;
        protected CameraConverter _cameraConverter;
        private GameObject _clickPoint;

        private readonly float _damping;
        private readonly float _linear;

        public MotionState(Rigidbody2D rigidbody, CameraConverter cameraConverter, (float, float) movingData)
        {
            currentRigidBody = rigidbody;
            _cameraConverter = cameraConverter;

            _damping = movingData.Item1;
            _linear = movingData.Item2;
        }

        public override void Enter() => base.Enter();

        public override void Exit()
        {
            base.Exit();
            UnityEngine.Object.Destroy(_clickPoint);
            _clickPoint = null;
        }

        public override void Update() => base.Update();

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
#if UNITY_EDITOR
            var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
#elif UNITY_ANDROID || UNITY_IOS
            var position = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0f);            
#endif
            Vector2 currentPosition;

            if (_clickPoint != null)
                currentPosition = _clickPoint.transform.position;
            else
                currentPosition = currentRigidBody.transform.position;

            Vector2 targetPosition = _cameraConverter.ScreenToWorldPoint(position);

            var direction = targetPosition - currentPosition;
            var velocity = direction / Time.fixedDeltaTime;

            velocity *= DampenFactor(_damping, Time.fixedDeltaTime);
            velocity = Vector2.MoveTowards(velocity, Vector2.zero, _linear * Time.fixedDeltaTime);

            currentRigidBody.velocity = velocity;
        }

        private float DampenFactor(float speed, float elipsed)
            => 1.0f - Mathf.Pow((float)Math.E, -speed * elipsed);
    }
}
