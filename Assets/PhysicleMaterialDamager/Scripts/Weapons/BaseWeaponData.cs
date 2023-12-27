using UnityEngine;

namespace PhysicleMaterialsDamager
{
    public abstract class BaseWeaponData : BaseData
    {
        [SerializeField] public GameObject ParticleObject;
        [SerializeField] public int Damage;
        [SerializeField] public AudioClip AuxiliaryClip;
        [SerializeField] public WeaponTypes WeaponType;
    }
}
