using UnityEngine;

namespace GameRecorder
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;

        private PhysicleData _data;
        private bool _playModeActive = false;

        public PhysicleData Data => _data;

        public void Construct() => _data = new PhysicleData(exist: true);

        public void SetPositionsOnPlayMode(PhysicleData data)
        {
            transform.position = data.Position;
            transform.rotation = data.Rotation;
            transform.localScale = data.Scale;
        }

        public void ContinueSimulation(PhysicleData data)
        {
            _rigidbody2D.isKinematic = false;
            _rigidbody2D.velocity = data.VelocityRB;
            _rigidbody2D.rotation = data.RotationRB;
            _rigidbody2D.angularDrag = data.AngularDragRB; 
            _rigidbody2D.angularVelocity = data.AngularVelocityRB; 
            _playModeActive = false;
        }

        public void StopSimulation()
        {
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.angularDrag = 0;
            _rigidbody2D.angularVelocity = 0;
            _rigidbody2D.isKinematic = true;
            _playModeActive = true;
        }

        private void Update()
        {
            if (_data.Exist == false) return;
            if (_playModeActive) return;

            var data = _data;

            data.Position = transform.position;
            data.Rotation = transform.rotation;
            data.Scale = transform.localScale;

            data.VelocityRB = _rigidbody2D.velocity;
            data.RotationRB = _rigidbody2D.rotation;
            data.AngularDragRB = _rigidbody2D.angularDrag;
            data.AngularVelocityRB = _rigidbody2D.angularVelocity;

            _data = data;
        }
    }
}
