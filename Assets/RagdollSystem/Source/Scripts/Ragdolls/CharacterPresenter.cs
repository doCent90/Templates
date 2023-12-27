using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PhysicleMaterialsDamager;
using Destructible2D;
using Core;

namespace RagdollSystem
{
    public class CharacterPresenter : PhysicleEntity, IIgnoreColliders
    {
        [SerializeField] private List<DestructAnimationOverrider> _destructAnimationOverriders;
        [SerializeField] private List<PhysicleMaterialsHandler> _physicleMaterialsHandlers;
        [SerializeField] private List<DestructionPartGenerator> _destructionPartGenerators;
        [SerializeField] private List<D2dDestructibleSprite> _destructibleSprites;
        [SerializeField] private List<SpriteRenderer> _spriteRenderers;
        [SerializeField] private List<Rigidbody2D> _rigidbodies;
        [SerializeField] private List<HealthPart> _healthParts;
        [SerializeField] private List<Collider2D> _colliders;
        [SerializeField] private List<Transform> _parts;
        [SerializeField] private Character _character;
        [SerializeField] private Animator _animator;
        [SerializeField] private Stamp _stamp;
        [SerializeField] private StandingSystem _standingSystem;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private float _gravityScaleDefault = 1;
        [SerializeField] private float _optimazeFactor = 1;
        [Header("Player")]
        [SerializeField] private ControllInterlayer _controllInterlayer;

        private SetupOwnCollidersIgnor _setupOwnCollidersIgnor;

        public IReadOnlyList<Collider2D> Colliders => _colliders;

        public override void Construct(GameComponets gameComponets)
        {
            base.Construct(gameComponets);
            _setupOwnCollidersIgnor = new();
            SetIgnoreColliders();

            _physicleMaterialsHandlers.ForEach(material => material.Construct());
            _rigidbodies.ForEach(body => body.gravityScale = _gravityScaleDefault);

            AllParts = _parts;

            _healthComponent.Construct(_character, _stamp);
            _destructionPartGenerators.ForEach(part => part.Construct());

            _destructAnimationOverriders.ForEach(part => part.Construct());
            _character.Construct(_controllInterlayer, _colliders, _spriteRenderers, _destructionPartGenerators, gameComponets.CameraConverter);

            if (_controllInterlayer != null)
                _controllInterlayer.Construct(gameComponets);
        }

        public void SetIgnoreColliders()
        {
            _colliders.Clear();
            _colliders.AddRange(GetComponentsInChildren<Collider2D>());
            _setupOwnCollidersIgnor.SetIgnoreOwnCollisions(_colliders, ignore: true);
        }

        #region Settings
#if UNITY_EDITOR
        [ContextMenu(nameof(SetComponents))]
        private void SetComponents()
        {
            _colliders.Clear();
            _rigidbodies.Clear();
            _healthParts.Clear();
            _destructibleSprites.Clear();
            _destructionPartGenerators.Clear();

            _colliders.AddRange(GetComponentsInChildren<Collider2D>());
            _healthParts.AddRange(GetComponentsInChildren<HealthPart>());
            _rigidbodies.AddRange(GetComponentsInChildren<Rigidbody2D>());
            _destructibleSprites.AddRange(GetComponentsInChildren<D2dDestructibleSprite>());
            _destructionPartGenerators.AddRange(GetComponentsInChildren<DestructionPartGenerator>());

            EditorUtility.SetDirty(this);
        }
#endif
        #endregion
    }
}
