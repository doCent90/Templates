using System.Collections.Generic;
using UnityEngine;

namespace RagdollSystem
{
    public class DestructAnimationOverrider : MonoBehaviour
    {
        [SerializeField] private Transform _bone;
        [SerializeField] private float _offset;
        [SerializeField] private List<HealthPart> _footsHealthParts;
        [SerializeField] private HealthComponent _healthComponent;

        private void OnDestroy() => _footsHealthParts.ForEach(part => part.Damaged -= SetFootAddRotation);

        public void Construct() => _footsHealthParts.ForEach(part => part.Damaged += SetFootAddRotation);

        private void SetFootAddRotation()
        {
            if (_healthComponent.HasDamagedLegs) return;

            _healthComponent.OnLegsDamaged();
            _bone.localEulerAngles += new Vector3(0, 0, _offset);
        }
    }
}
