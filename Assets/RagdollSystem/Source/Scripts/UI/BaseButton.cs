using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RagdollSystem
{
    [RequireComponent(typeof(Button))]
    public class BaseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Button _button;

        public event Action OnClick;
        public event Action OnDown;
        public event Action OnUp;

        private void Start() => Init();
        private void OnDestroy() => _button.onClick.RemoveAllListeners();

        public void OnPointerDown(PointerEventData eventData) => OnDown?.Invoke();
        public void OnPointerUp(PointerEventData eventData) => OnUp?.Invoke();

        protected virtual void Init() => _button.onClick.AddListener(OnClickButton);
        protected virtual void OnClickButton() => OnClick?.Invoke();
    }
}
