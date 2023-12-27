using Core;
using System;
using UnityEngine;

namespace RagdollSystem
{
    public class PlayableCharacterMark : MonoBehaviour, IDragAndDrop
    {
        [SerializeField] private Character _character;
        private readonly DragAndDropAttribute _dragAndDropAttribute = DragAndDropAttribute.OnlyClick;

        public event Action OnClick;

        DragAndDropAttribute IDragAndDrop.DragAndDropAttribute => _dragAndDropAttribute;
        Transform IDragAndDrop.Transform => transform;

        void IDragAndDrop.Click() => OnClick?.Invoke();
        bool IDragAndDrop.TryBeginDrag(Vector3 clickPoint) => false;
        bool IDragAndDrop.TryDrag() => false;
        void IDragAndDrop.Drop() { }
    }
}
