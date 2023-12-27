using UnityEngine;

namespace PhysicleMaterialsDamager
{
    public class PhysicleMaterialList : MonoBehaviour
    {
        [SerializeField] private PhysicleMaterial _fire;
        [SerializeField] private PhysicleMaterial _water;
        [SerializeField] private PhysicleMaterial _chemical;
        [SerializeField] private PhysicleMaterial _cold;

        private PhysicMaterialType _type = PhysicMaterialType.Water;

        public void SetMaterial(PhysicMaterialType type)
        {
            if (_type != type)
            {
                _fire.gameObject.SetActive(false);
                _water.gameObject.SetActive(false);
                _chemical.gameObject.SetActive(false);
                _cold.gameObject.SetActive(false);

                switch (type)
                {
                    case PhysicMaterialType.Water: _water.gameObject.SetActive(true); break;
                    case PhysicMaterialType.Cold: _cold.gameObject.SetActive(true); break;
                    case PhysicMaterialType.Lava: _fire.gameObject.SetActive(true); break;
                    case PhysicMaterialType.Chemical: _chemical.gameObject.SetActive(true); break;
                    default: break;
                }
            }

            _type = type;
        }
    }
}
