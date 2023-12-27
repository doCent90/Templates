using UnityEngine;

namespace Core
{
    public enum DragAndDropAttribute
    {
        None = 0,
        OnlyClick = 1,
    }

    public interface IDragAndDrop
    {
        public DragAndDropAttribute DragAndDropAttribute { get; }
        public Transform Transform { get; }

        public void Click();
        public bool TryBeginDrag(Vector3 clickPoint);
        public bool TryDrag();
        public void Drop();
    }
}