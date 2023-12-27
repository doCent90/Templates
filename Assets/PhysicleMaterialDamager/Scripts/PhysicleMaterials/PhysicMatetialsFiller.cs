using UnityEngine;

namespace PhysicleMaterialsDamager
{
    public class PhysicMatetialsFiller : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _fire;
        [SerializeField] private Color _fireColor;
        [SerializeField] private ParticleSystem _water;
        [SerializeField] private Color _waterColor;
        [SerializeField] private ParticleSystem _chemical;
        [SerializeField] private Color _chemiclaColor;
        [SerializeField] private ParticleSystem _cold;
        [SerializeField] private Color _coldColor;
        [Header("Wepon")][SerializeField] private MonoBehaviour _weaponComponent;
        [SerializeField] private SpriteRenderer _baloonSprite;

        private IElementalWeapon _weapon;

        private void Start() => _weapon = (IElementalWeapon)_weaponComponent;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PhysicleMaterial material) && material.Filler)
            {
                switch (material.Type)
                {
                    case PhysicMaterialType.Water: _weapon.SetFXBullets(_water, PhysicMaterialType.Water); SetColor(_waterColor); break;
                    case PhysicMaterialType.Cold: _weapon.SetFXBullets(_cold, PhysicMaterialType.Cold); SetColor(_coldColor); break;
                    case PhysicMaterialType.Lava: _weapon.SetFXBullets(_fire, PhysicMaterialType.Lava); SetColor(_fireColor); break;
                    case PhysicMaterialType.Chemical: _weapon.SetFXBullets(_chemical, PhysicMaterialType.Chemical); SetColor(_chemiclaColor); break;
                }
            }
        }

        private void SetColor(Color color) => _baloonSprite.color = color;
    }
}
