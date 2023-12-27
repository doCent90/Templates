using UnityEngine;

namespace Weapons
{
    public class ResetPosition : MonoBehaviour
    {
        [SerializeField] private Vector3 _localPosition;

        [ContextMenu(nameof(ResetPos))]
        public void ResetPos()
        {
            transform.localPosition = _localPosition;
        }
    }
}
