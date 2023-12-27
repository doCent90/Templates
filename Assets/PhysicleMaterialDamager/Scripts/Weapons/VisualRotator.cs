using System.Collections.Generic;
using UnityEngine;

namespace PhysicleMaterialsDamager
{
    public class VisualRotator : MonoBehaviour
    {
        [SerializeField] private Vector3Int _flipRotation;
        [SerializeField] private Vector3Int _flipScale;

        [SerializeField] private List<Transform> _targets;

        public void Rotate()
        {
            foreach (var target in _targets)
            {
                target.localScale = new Vector3(target.localScale.x * _flipScale.x, target.localScale.y * _flipScale.y, target.localScale.z * _flipScale.z);
                target.localRotation = Quaternion.Euler(target.localRotation.eulerAngles.x + 180 * _flipRotation.x, target.localRotation.eulerAngles.y + 180 * _flipRotation.y, target.localRotation.eulerAngles.z + 180 * _flipRotation.z);
            }
        }
    }
}
