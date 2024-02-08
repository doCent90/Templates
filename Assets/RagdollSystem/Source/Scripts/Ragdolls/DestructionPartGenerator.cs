using Destructible2D;
using UnityEngine;

namespace RagdollSystem
{
    public class DestructionPartGenerator : MonoBehaviour
    {
        private const float MinDamageValue = 0.95f;
        private const float Force = 5f;
        private const float Staright = 0.5f;
        [Header("Destructions Part")]
        [SerializeField] private D2dDestructibleSprite _destructibleSkin;
        [SerializeField] private D2dDestructibleSprite _destructibleMeat;
        [SerializeField] private Transform _bone;
        [SerializeField] private int _layer;
        [SerializeField] private float _destroyDelay;

        private bool _damagedSkin = false;
        private bool _damagedMeat = false;
        private bool _splitted = false;

        private void OnDestroy()
        {
            _destructibleSkin.OnModified -= OnDamagedSkin;
            _destructibleMeat.OnModified -= OnDamagedMeat;
            _destructibleSkin.OnSplit -= OnSplit;
            _destructibleMeat.OnSplit -= OnSplit;
        }

        public void Construct()
        {
            _destructibleSkin.OnModified += OnDamagedSkin;
            _destructibleMeat.OnModified += OnDamagedMeat;
            _destructibleSkin.OnSplit += OnSplit;
            _destructibleMeat.OnSplit += OnSplit;
            _destructibleMeat.gameObject.SetActive(false);
            _bone.gameObject.SetActive(false);
        }

        public void OnDamaged()
        {
            if (_damagedMeat && _damagedSkin)
                return;

            if (_destructibleSkin.AlphaRatio <= MinDamageValue && _destructibleMeat.gameObject.activeInHierarchy == false)
            {
                _destructibleMeat.gameObject.SetActive(true);
                _damagedSkin = true;
            }

            if (_destructibleMeat.AlphaRatio <= MinDamageValue && _bone.gameObject.activeInHierarchy == false)
            {
                _bone.gameObject.SetActive(true);
                _damagedMeat = true;
            }
        }

        public void EnableAll()
        {
            _destructibleMeat.gameObject.SetActive(true);
            _bone.gameObject.SetActive(true);
        }

        public void DisableAll()
        {
            _destructibleSkin.gameObject.SetActive(false);
            _destructibleMeat.gameObject.SetActive(false);
            _bone.gameObject.SetActive(false);
        }

        private void OnDamagedSkin(D2dRect obj)
        {
            if (_destructibleSkin.AlphaRatio <= MinDamageValue && _destructibleMeat.gameObject.activeInHierarchy == false)
            {
                _destructibleMeat.gameObject.SetActive(true);
                _destructibleSkin.OnModified -= OnDamagedSkin;
            }
        }

        private void OnDamagedMeat(D2dRect obj)
        {
            if (_destructibleMeat.AlphaRatio <= MinDamageValue && _bone.gameObject.activeInHierarchy == false)
            {
                _destructibleMeat.OnModified -= OnDamagedMeat;
                _bone.gameObject.SetActive(true);
            }
        }

        private void OnSplit(D2dDestructible piece)
        {
            if (_splitted) return;

            _splitted = true;
            piece.gameObject.layer = _layer;
            piece.transform.parent = null;

            Rigidbody2D rigidbody2D = piece.gameObject.AddComponent<Rigidbody2D>();
            if (piece.TryGetComponent(out D2dPolygonCollider targetPlygon) == false)
            {
                var polygon = piece.gameObject.AddComponent<D2dPolygonCollider>();
            }
            else
            {
                targetPlygon.CellSize = D2dPolygonCollider.CellSizes.Square64;
                targetPlygon.Straighten = Staright;
            }

            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2D.velocity = Random.insideUnitCircle * Force;

            Destroy(piece.GetComponent<D2dSplitter>());
            piece.gameObject.AddComponent<CharacterDestructionPart>().AutoDestroy(_destroyDelay);
        }
    }
}
