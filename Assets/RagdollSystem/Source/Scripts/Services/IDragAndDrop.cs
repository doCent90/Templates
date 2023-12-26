using UnityEngine;

namespace RagdollSystem
{
    public enum DragAndDropAttribute
    {
        None = 0,
        OnlyClick = 1,
    }

    internal interface IDragAndDrop
    {
        internal DragAndDropAttribute DragAndDropAttribute { get; }
        internal Transform Transform { get; }

        internal void Click();
        internal bool TryBeginDrag(Vector3 clickPoint);
        internal bool TryDrag();
        internal void Drop();
    }

    public interface IDragService : IToggleService
    {
        public bool IsHolding { get; }

        public RaycastHit2D[] GetRaycastObjects(Vector3 position);
    }

    public interface IToggleService
    {
        void EnableService() { }

        void DisableService() { }
    }
}