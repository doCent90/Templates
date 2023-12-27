using System;
using UnityEngine;
using Destructible2D;
using Core;

namespace PhysicleMaterialsDamager
{
    public class DestructibleObstacle : MonoBehaviour, IDamagable
    {
        private const float MinToBroken = 0.75f;
        [SerializeField] private ObstacleDestructiblePart _obstacleDestructiblePart;
        [SerializeField] private float _healthValue = 20f;
        [SerializeField] private int _optimizeFactor = 2;
        [SerializeField] private int _brokeVelocity = 10;
        [SerializeField] private bool _onlyOneFracture;
        [SerializeField] private bool _brokeOnDamage;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private DestructableData _destructableData;
        [SerializeField] private D2dFracturer _fracturer;
        [SerializeField] private D2dSplitter _splitter;
        [SerializeField] private D2dDestructibleSprite _destructibleSprite;
        [SerializeField] private D2dCalculateMass _calculateMass;
        [SerializeField] private PhysicleMaterialsHandler _physicsMaterialsHandler;

        private Action<Rigidbody2D, Collider2D> _broken;
        private DestructableHealth _health;
        private IDestructionQueueHandler _destructionQueueHandler;
        private float _massToFadeOut;
        private bool _isBroken;

        public Rigidbody2D SelfBody { get; private set; }
        public DestructableSplitter DestructableSplitter { get; private set; }
        public PhysicleMaterialsHandler PhysicsMaterialsHandler => _physicsMaterialsHandler;
        public ObstacleDestructiblePart ObstacleDestructible => _obstacleDestructiblePart;
        public DestructableData DestructableData => _destructableData;
        public D2dFracturer Fracturer => _fracturer;
        public D2dSplitter Splitter => _splitter;

        private void OnDestroy() => _destructibleSprite.OnSplit -= DestructableSplitter.OnDestructibleSplited;

        public void Construct(CameraConverter cameraConverter, IDestructionQueueHandler destructionQueueHandler, Rigidbody2D selfBody)
        {
            SelfBody = selfBody;
            _massToFadeOut = selfBody.mass / _fracturer.MaxPoints;
            _isBroken = false;
            _destructionQueueHandler = destructionQueueHandler;
            DestructableSplitter = new(_broken, _collider, transform, _obstacleDestructiblePart, _fracturer, _destructibleSprite,
                _calculateMass, SelfBody, cameraConverter, _destructionQueueHandler, _massToFadeOut, _onlyOneFracture, _optimizeFactor);
            _destructibleSprite.OnSplit += DestructableSplitter.OnDestructibleSplited;
            _health = new(_healthValue, DestructableSplitter);
            _fracturer.enabled = false;
            _splitter.enabled = false;
            DestructableSplitter.Rebuild();
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (_isBroken) return;

            if (col.relativeVelocity.magnitude > _brokeVelocity)
            {
                _fracturer.enabled = true;
                _splitter.enabled = true;
                DestructableSplitter.Broke();
            }
        }

        public void ApplyDamage(int value, bool force, DamageType type, Vector2 contact = default)
        {
            if (_isBroken || type == DamageType.None)
                return;

            if (type == DamageType.Explosion || type == DamageType.Laser || type == DamageType.Range || type == DamageType.Melee)
                _health.TakeDamage(value);

            if (type == DamageType.Laser)
            {
                _fracturer.enabled = true;
                _splitter.enabled = true;
            }

            if (_destructibleSprite.AlphaRatio < MinToBroken)
            {
                _fracturer.enabled = true;
                _splitter.enabled = true;
                DestructableSplitter.Broke();
            }

            if (_brokeOnDamage)
                DestructableSplitter.Broke();
        }

        public void CriticalDamage(bool split) { }

        #region Settings
#if UNITY_EDITOR
        [ContextMenu(nameof(SetComponents))]
        private void SetComponents()
        {
            CreateComponents();
            GetComponent<SpriteRenderer>().sortingLayerName = "Obstacle";
            _calculateMass = GetComponent<D2dCalculateMass>();
            _destructibleSprite = GetComponent<D2dDestructibleSprite>();
            _fracturer = GetComponent<D2dFracturer>();
            _obstacleDestructiblePart = gameObject.GetComponent<ObstacleDestructiblePart>();

            _destructibleSprite.ChannelsRaw = D2dDestructibleSprite.ChannelType.AlphaWithWhiteRGB;
            _destructibleSprite.Pixels = D2dDestructible.PixelsType.PixelatedBinary;
            _destructibleSprite.Rebuild();
        }

        private void CreateComponents()
        {
            if (!TryGetComponent<Rigidbody2D>(out var rigidbody2D))
            {
                gameObject.AddComponent<Rigidbody2D>();
            }

            if (!TryGetComponent<D2dDestructibleSprite>(out var destructibleSprite))
            {
                gameObject.AddComponent<D2dDestructibleSprite>();
            }

            if (!TryGetComponent<D2dPolygonCollider>(out var d2dPolygonCollider))
            {
                gameObject.AddComponent<D2dPolygonCollider>();
            }

            if (!TryGetComponent<D2dFracturer>(out var d2dFracturer))
            {
                gameObject.AddComponent<D2dFracturer>();
            }

            if (!TryGetComponent<ObstacleDestructiblePart>(out var obstacleDestructiblePart))
            {
                gameObject.AddComponent<ObstacleDestructiblePart>();
            }

            if (!TryGetComponent<D2dSplitter>(out var d2dSplitter))
            {
                gameObject.AddComponent<D2dSplitter>();
            }

            if (!TryGetComponent<D2dCalculateMass>(out var calculateMass))
            {
                gameObject.AddComponent<D2dCalculateMass>();
            }
        }
#endif
        #endregion
    }
}
