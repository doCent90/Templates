using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public class CameraConverter : MonoBehaviour
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }

        public Vector3 GetDifference(Vector3 a, Vector3 b) =>
            MainCamera.ScreenToWorldPoint(a) - b;

        public float GetDistance(Vector2 a, Vector2 b) =>
            Vector2.Distance(ScreenToWorldPoint(a), ScreenToWorldPoint(b));

        public Vector3 ScreenToWorldPoint(Vector3 vector)
            => MainCamera.ScreenToWorldPoint(vector);

        public bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };

            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
