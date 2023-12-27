using UnityEngine;

namespace PhysicleMaterialsDamager
{
    [CreateAssetMenu(fileName = "New Pickable Weapon Data", menuName = "ScriptableObjects/PickableWeaponData", order = 1)]
    public class PickableWeaponData : BaseWeaponData
    {
        [SerializeField] public float RotationAbs;
        [SerializeField] public Vector2 LocalHandPosition;
        [SerializeField] public float LocalHandRotation;
        [SerializeField] public Vector2 FrontHandPosition;
        [SerializeField] public Vector2 BackHandPosition;
        [SerializeField] public Vector2 RotationBorder;
        [SerializeField] public float AngelOffset;
        [SerializeField] public bool CreateJoint;
        [SerializeField] public bool SecondPosition;
        public Vector2 SecondFrontHandPosition;
        public Vector2 SecondBackHandPosition;
        public Vector2 SecondPositionAngel;
    }
}
