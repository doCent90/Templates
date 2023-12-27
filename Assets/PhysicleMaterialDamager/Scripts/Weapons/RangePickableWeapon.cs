using Core;
using System.Collections;
using UnityEngine;

namespace PhysicleMaterialsDamager
{
    public class RangePickableWeapon : PickableWeapon, IElementalWeapon
    {
        [SerializeField] protected GameObject _gunBarrel;
        [SerializeField] private Bullet _bulletTamplate;
        [SerializeField] private ParticleSystem _fxBullets;
        [SerializeField] private float _fireDelay;
        [SerializeField] private bool _autoFire;
        [SerializeField] private float _recoilAngel;
        [SerializeField] private float _recoilDuration;
        [SerializeField] private float _shakeMultiplier = 3;
        private PhysicMaterialType _materialType = PhysicMaterialType.Water;

        private Coroutine _autoShootRoutine;
        private bool _isAttack;

        public override void StartAttack()
        {
            base.StartAttack();
            if (_isAttack) return;
            StartAutoShoot();
        }

        public override void StopAttack()
        {
            base.StopAttack();
            if (!_isAttack) return;
            StopAutoShoot();
        }

        protected override bool TryBeginDrag()
        {
            StateMachine.ChangeState(new RangeWeaponControlState(Rigidbody, CameraConverter, gameObject, this, (Damping, Linear)));

            if (_autoFire)
                StartAutoShoot();

            return true;
        }

        protected override void Drop()
        {
            base.Drop();
            StopAutoShoot();
            StateMachine.ChangeState(DropState);
        }

        public void StartAutoShoot()
        {
            StopAutoShoot();
            _autoShootRoutine = StartCoroutine(AutoShoot());
            _isAttack = true;
        }

        public void StopAutoShoot()
        {
            if (_autoShootRoutine != null)
            {
                StopCoroutine(_autoShootRoutine);
                _autoShootRoutine = null;

                if (_fxBullets != null)
                    _fxBullets.Stop();
            }

            _isAttack = false;
        }

        public void SetFXBullets(ParticleSystem particle, PhysicMaterialType materialType)
        {
            _fxBullets.Stop();
            _materialType = materialType;
            _fxBullets = particle;
        }

        protected override void OnTouch(Collision2D collision) { }

        private IEnumerator AutoShoot()
        {
            float lastShootTime = 0;
            while (true)
            {
                if (Time.time - lastShootTime >= _fireDelay)
                {
                    Shoot();
                    lastShootTime = Time.time;
                }
                yield return null;
            }
        }

        public virtual void Shoot()
        {
            ShootFeedback();
            if (_fxBullets != null)
                _fxBullets.Play();

            var bullet = Instantiate(_bulletTamplate, _gunBarrel.transform.position, _gunBarrel.transform.rotation);
            bullet.MaterialList.SetMaterial(_materialType);
            bullet.Construct();
            bullet.Launch();
            bullet.SetupDamage(Data.Damage);
        }

        protected void ShootFeedback() { }
    }
}
