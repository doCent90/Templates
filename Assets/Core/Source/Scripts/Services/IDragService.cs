using UnityEngine;

namespace Core
{
    public interface IDragService : IToggleService
    {
        public bool IsHolding { get; }

        public RaycastHit2D[] GetRaycastObjects(Vector3 position);
    }
}