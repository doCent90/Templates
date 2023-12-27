using UnityEngine;

namespace PhysicleMaterialsDamager
{
    public interface IElementalWeapon
    {
        void SetFXBullets(ParticleSystem particle, PhysicMaterialType materialType);
    }
}
