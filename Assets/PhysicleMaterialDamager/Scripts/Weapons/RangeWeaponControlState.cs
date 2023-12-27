using UnityEngine;
using Core;

namespace PhysicleMaterialsDamager
{
    public class RangeWeaponControlState : MotionState
    {
        private readonly GameObject _rootGameObject;
        private readonly RangePickableWeapon _rangePickableWeapon;
        private bool _isStartShooting;

        public RangeWeaponControlState(Rigidbody2D rigidBody, CameraConverter cameraConverter, GameObject rootGameObject, RangePickableWeapon rangePickableWeapon, (float, float) movingData) : base(rigidBody, cameraConverter, movingData)
        {
            _rootGameObject = rootGameObject;
            _rangePickableWeapon = rangePickableWeapon;
        }

        public override void Update()
        {
            base.Update();

            if (Input.touchCount <= 1)
            {
                if (_isStartShooting)
                {
                    _isStartShooting = false;
                    _rangePickableWeapon.StopAutoShoot();
                }

                return;
            }

            var touch = Input.GetTouch(1);

            Vector3 difference =
                _cameraConverter.GetDifference(touch.position, _rootGameObject.transform.position);

            var rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            _rootGameObject.transform.rotation = Quaternion.Euler(0f, 0f, rotateZ);

            var distance = _cameraConverter.GetDistance(touch.position, Input.GetTouch(0).position);

            if (distance >= 2 && !_isStartShooting)
            {
                _isStartShooting = true;
                _rangePickableWeapon.StartAutoShoot();
            }

            if (distance < 2 && _isStartShooting)
            {
                _isStartShooting = false;
                _rangePickableWeapon.StopAutoShoot();
            }
        }
    }
}