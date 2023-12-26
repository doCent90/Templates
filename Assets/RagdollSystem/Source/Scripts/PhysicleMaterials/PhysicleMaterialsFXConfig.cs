using UnityEngine;

namespace RagdollSystem
{
    [CreateAssetMenu(fileName = "PhysMaterial FX", menuName = "create PhysMaterial FX", order = 51)]
    public class PhysicleMaterialsFXConfig : ScriptableObject
    {
        [field: SerializeField] public ParticleSystem Fire { get; private set; }
        [field: SerializeField] public ParticleSystem Cold { get; private set; }
    }
}
