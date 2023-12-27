using UnityEngine;
using Core;
using Destructible2D;

namespace PhysicleMaterialsDamager
{
    public class PhysicleMaterial : PhysicleEntity, ICoroutine
    {
        [SerializeField] private Collider2D _collider;
        [SerializeField] private bool _staticMaterial = false;
        [SerializeField] private float _randomPos = 0f;

        [field: SerializeField] public DestructableData D2Data { get; private set; }
        [field: SerializeField] public DestructableData D2DataSecond { get; private set; }
        [field: SerializeField] public PhysicMaterialType Type { get; private set; }
        [field: SerializeField] public float Temperature { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public bool Filler { get; private set; } = false;

        private void OnTriggerEnter2D(Collider2D collision) => OnCollided(collision);

        private void OnCollided(Collider2D collision)
        {
            if (collision.attachedRigidbody != null
                && collision.attachedRigidbody.TryGetComponent(out IPhysicleMaterialsHandler materialsHandler))
                materialsHandler.OnCollided(_collider, this, _randomPos, _staticMaterial);
        }
    }
}
