using UnityEngine;

namespace PhysicleMaterialsDamager
{
    public interface IPhysicleMaterialsHandler
    {
        PhysStateType State { get; }
        float TemperatureValue { get; }

        void OnCollided(Collider2D collider, PhysicleMaterial material, float randomPosbool, bool staticMaterial = false);
        void OnExit();
    }
}
