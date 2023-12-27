using UnityEngine;
using UnityEngine.Serialization;
using Assets.Lemon.Scripts.Weapons.RangeLogic;
using Destructible2D;

namespace PhysicleMaterialsDamager
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    internal class Bullet : MonoBehaviour
    {
        private const int EmiterCount = 3;

        [SerializeField] private int _speed;
        [SerializeField] private bool _pierce = false;
        [SerializeField] private DestructableData _destructableData;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [Range(0, 1)][SerializeField] private float _lifetime = 0;

        private int _damage;
        private bool _complete;

        [field: SerializeField] public PhysicleMaterialList MaterialList { get; private set; }
        [field: SerializeField] public BulletType BulletType { get; private set; }
        [field: SerializeField] public DamageType DamageType { get; private set; }

        public void Construct() { }

        public void Launch()
        {
            if (_lifetime > 0)
                Invoke(nameof(Despawn), _lifetime);

            _rigidbody2D.velocity = transform.up * _speed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_complete) return;
            if (collision.collider.isTrigger) return;

            if (_pierce == false)
            {
                _complete = true;
                Despawn();
            }

            if (BulletType != BulletType.Elemental)
                OnCollided(collision);
        }

        public void SetupDamage(int dataDamage) => _damage = dataDamage;

        private void OnCollided(Collision2D collision)
        {
            if (collision.collider.attachedRigidbody != null && collision.collider.attachedRigidbody.TryGetComponent(out IDamagable component))
                component.ApplyDamage(_damage, force: false, DamageType, transform.position);
        }

        private void Despawn()
        {
            _complete = false;
            Destroy(gameObject);
        }
    }
}
