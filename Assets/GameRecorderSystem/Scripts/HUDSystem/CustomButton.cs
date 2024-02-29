using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameRecorder
{
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnClickDown;
        public event Action OnClickUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnClickDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnClickUp?.Invoke();
        }
    }
}
