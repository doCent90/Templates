using UnityEngine;
using Core;
using Destructible2D;

namespace PhysicleMaterialsDamager
{
    public partial class PhysicleMaterialsHandler : PhysicleEntity, IPhysicleMaterialsHandler, ICoroutine
    {
        private const float DefaultTemperatureValue = 25f;
        private const float FireTempFactor = 200f;

        [SerializeField] private MonoBehaviour _healthsComponent;
        [SerializeField] private PhysicleMaterialsFXConfig _fXConfig;
        [SerializeField] private Stamp _stamp;

        private MaterialLooper _looper;
        private IDamagable _damagable;

        private void OnTriggerExit2D(Collider2D collision) => OnExit();

        public PhysStateType State { get; private set; } = PhysStateType.Normal;
        public float TemperatureValue { get; private set; } = DefaultTemperatureValue;

        public override void Construct()
        {
            _damagable = (IDamagable)_healthsComponent;
            _looper = new(this, this);
        }

        public void OnCollided(Collider2D collider, PhysicleMaterial material, float randomPos, bool staticMaterial = false)
        {
            switch (material.Type)
            {
                case PhysicMaterialType.Water: OnWaterInteracted(); break;
                case PhysicMaterialType.Cold: OnColdInteracted(material, collider); break;
                case PhysicMaterialType.Fire: OnFireInteracted(material, collider, randomPos); break;
                case PhysicMaterialType.Lava: OnLavaInteracted(material, collider, randomPos); break;
                case PhysicMaterialType.Chemical: OnChemicalInteracted(material, collider, randomPos); break;
            }

            SetTemperature(material);

            if (staticMaterial)
                OnStay(collider, material);
        }

        public void OnStay(Collider2D collider, PhysicleMaterial material) => _looper.Start(collider, material);
        public void OnExit() => _looper.Stop();
        private void OnWaterInteracted() => State = PhysStateType.Normal;
    }

    public partial class PhysicleMaterialsHandler : IPhysicleMaterialsHandler
    {
        private const float SpeedMultiplierFactor = 0.1f;
        private ParticleSystem _fire;
        private ParticleSystem _cold;

        public void ResetFX()
        {
            if (_fire != null) _fire.Stop();
            if (_cold != null) _cold.Stop();
        }

        private void SetStamp(PhysicleMaterial material, Vector2 position)
        {
            _stamp.Execute(material.D2Data, position);

            if (material.D2DataSecond != null)
                _stamp.Execute(material.D2DataSecond, position);
        }

        private void OnColdInteracted(PhysicleMaterial material, Collider2D collider)
        {
            State = PhysStateType.Freeze;
            _damagable.ApplyDamage(material.Damage, force: true, DamageType.Cold);
            Vector2 collisionPoint = collider.ClosestPoint(transform.position);
            SetStamp(material, collisionPoint);
        }

        private void OnLavaInteracted(PhysicleMaterial material, Collider2D collider, float randomPos)
        {
            State = PhysStateType.Burning;
            OnFireInteracted(material, collider, randomPos);
        }

        private void OnFireInteracted(PhysicleMaterial material, Collider2D collider, float randomPos)
        {
            if(material == null) return;
            State = PhysStateType.Burning;
            _damagable.ApplyDamage(material.Damage, force: true, DamageType.Fire);
            Vector2 collisionPoint = collider.ClosestPoint(transform.position);
            collisionPoint.x += Random.Range(-randomPos, randomPos);
            collisionPoint.y += Random.Range(-randomPos, randomPos);
            SetStamp(material, collisionPoint);
        }

        private void OnChemicalInteracted(PhysicleMaterial material, Collider2D collider, float randomPos)
        {
            State = PhysStateType.Chemical;
            _damagable.ApplyDamage(material.Damage, force: true, DamageType.Chemical);

            Vector2 collisionPoint = collider.ClosestPoint(transform.position);
            collisionPoint.x += Random.Range(-randomPos, randomPos);
            collisionPoint.y += Random.Range(-randomPos, randomPos);

            SetStamp(material, collisionPoint);
        }

        private void SetTemperature(PhysicleMaterial material)
        {
            TemperatureValue = material.Temperature;

            if (_fire == null && TemperatureValue > FireTempFactor)
            {
                _fire = Instantiate(_fXConfig.Fire, transform.position, Quaternion.identity, transform);
                _fire.Play();

                if(_cold != null)
                    Destroy(_cold.gameObject);
            }

            if(_cold == null && TemperatureValue < 0)
            {
                _cold = Instantiate(_fXConfig.Cold, transform.position, Quaternion.identity, transform);
                _cold.Play();
            }

            if (_fire != null && TemperatureValue < FireTempFactor)
                _fire.Stop();

            if(_cold != null && TemperatureValue > 0)
            {
                _cold.Stop();
                Destroy(_cold.gameObject);
            }
        }
    }
}
