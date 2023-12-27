using System;
using UnityEngine;

namespace Core
{
    public class DragAndDrop : MonoBehaviour, IDragService
    {
        private const float MinHoldMagnitude = 7f;
        private const float ClickTime = 0.25f;

        private readonly RaycastHit2D[] _raycastHits = new RaycastHit2D[10];
        private readonly IDragAndDrop[] _onlyClick = new IDragAndDrop[10];
        private IDragAndDrop _currentItem;
        private Vector3 _currentClickPoint;
        private Vector3 _lastClickPosition;
        private bool _isUI;
        private float _lastClickTime;

        private Camera _mainCamera;
        private CameraConverter _cameraConverter;

        public bool IsHolding { get; private set; }

        public void Construct(CameraConverter cameraConverter)
        {
            _cameraConverter = cameraConverter;
            _mainCamera = _cameraConverter.MainCamera;
        }

        void IToggleService.EnableService()
        {
            if (this == null) return;
            enabled = true;
        }

        void IToggleService.DisableService()
        {
            if (this == null) return;
            enabled = false;
        }

        private void Update()
        {
            _isUI = _cameraConverter.IsPointerOverUIObject();

#if UNITY_EDITOR

            if (Input.GetMouseButtonDown(0) && !_isUI)
                OnFingerDown(Input.mousePosition);

            if (Input.GetMouseButton(0))
                OnFingerHold();

            if (Input.GetMouseButtonUp(0))
                OnFingerUp();

#elif UNITY_ANDROID || UNITY_IOS

            if (Input.touchCount <= 0)
                return;

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && !_isUI)
                OnFingerDown(touch.position);
            if (touch.phase == TouchPhase.Moved)
                OnFingerHold();
            if (touch.phase == TouchPhase.Ended)
                OnFingerUp();
#endif
        }

        private void OnFingerDown(Vector3 position)
        {
            GetRaycastObjects(position);
            int onlyClickId = 0;

            foreach (RaycastHit2D item in _raycastHits)
            {
                if (item.transform == null) continue;
                if (item.transform.TryGetComponent(out IDragAndDrop dragItem))
                {
                    _lastClickTime = Time.time;
                    if (dragItem.DragAndDropAttribute == DragAndDropAttribute.OnlyClick)
                    {
                        _onlyClick[onlyClickId] = dragItem;
                        onlyClickId++;
                        continue;
                    }

                    _lastClickPosition = position;
                    _currentItem = dragItem;
                    _currentClickPoint = item.point;
                }
            }
        }

        public RaycastHit2D[] GetRaycastObjects(Vector3 position)
        {
            Ray ray = _mainCamera.ScreenPointToRay(position);
            Array.Clear(_raycastHits, 0, _raycastHits.Length);
            Array.Clear(_onlyClick, 0, _onlyClick.Length);
            Physics2D.GetRayIntersectionNonAlloc(ray, _raycastHits);

            return _raycastHits;
        }

        private void OnFingerHold()
        {
            if (_currentItem == default)
                return;

            if (IsHolding == false)
            {
                Vector3 holdDelta = _lastClickPosition - Input.mousePosition;

                if (holdDelta.magnitude < MinHoldMagnitude)
                    return;

                if (_currentItem.TryBeginDrag(_currentClickPoint))
                {
                    IsHolding = true;
                }
                else
                {
                    _currentItem = null;
                    return;
                }
            }

            if (_currentItem.TryDrag() == false)
            {
                _currentItem = null;
                IsHolding = false;
            }
        }

        private void OnFingerUp()
        {
            if (_currentItem == default)
            {
                TryClick();
                return;
            }

            if (IsHolding)
                _currentItem.Drop();
            else
                TryClick();

            _currentItem = null;
            IsHolding = false;
        }

        private void TryClick()
        {
            if (Time.time <= _lastClickTime + ClickTime)
            {
                _currentItem?.Click();
                IDragAndDrop onlyClick = GetClosestOnlyClick();
                onlyClick?.Click();
            }
        }

        private IDragAndDrop GetClosestOnlyClick()
        {
            float minDistance = float.MaxValue;
            int closestId = 0;

            for (int i = 0; i < _onlyClick.Length; i++)
            {
                if (_onlyClick[i] == null) continue;
                float distance = Vector2.Distance(_lastClickPosition, _onlyClick[i].Transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestId = i;
                }
            }

            return _onlyClick[closestId];
        }
    }
}
