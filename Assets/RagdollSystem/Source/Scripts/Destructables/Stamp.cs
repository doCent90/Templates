using System.Collections.Generic;
using Destructible2D;
using RagdollSystem;
using UnityEngine;

namespace Destrutible
{
    public class Stamp : MonoBehaviour
    {
        [SerializeField] private Vector3 _localOffset;

        public void Execute(DestructableData sourceData, DestructableData targetData, Color color, Vector3 position = default, Quaternion rotation = default, List<GameObject> exclusions = default)
        {
            DestructableData data;

            if (sourceData != null)
                data = targetData.Priority > sourceData.Priority ? targetData : sourceData;
            else
                data = targetData;

            if (data.IsDestructable == false)
                return;

            float stampAngle = data.StampRandomDirection == true ? Random.Range(-180.0f, 180.0f) : 0.0f;

            if (rotation != default)
                stampAngle = rotation.eulerAngles.z;

            D2dStamp.All(data.StampPaint, position == default ? transform.TransformPoint(transform.localPosition + _localOffset) : position,
                data.StampSize, stampAngle, data.StampShape, color, data.Mask, exclusions);
        }

        public void Execute(DestructableData sourceData, DestructableData targetData, Vector3 position = default, Quaternion rotation = default, List<GameObject> exclusions = default)
        {
            DestructableData data;

            if (sourceData != null)
                data = targetData.Priority > sourceData.Priority ? targetData : sourceData;
            else
                data = targetData;

            if (data.IsDestructable == false)
                return;

            float stampAngle = data.StampRandomDirection == true ? Random.Range(-180.0f, 180.0f) : 0.0f;

            if (rotation != default)
                stampAngle = rotation.eulerAngles.z;

            D2dStamp.All(data.StampPaint, position == default ? transform.TransformPoint(transform.localPosition + _localOffset) : position,
                data.StampSize, stampAngle, data.StampShape, targetData.DestructColor, data.Mask, exclusions);
        }

        public void Execute(DestructableData data, Vector3 size = default, Vector3 position = default, Quaternion rotation = default)
        {
            if (data.IsDestructable == false)
                return;

            Vector3 targetSize = size != default ? size : (Vector3)data.StampSize;
            float stampAngle = data.StampRandomDirection == true ? Random.Range(-180.0f, 180.0f) : 0.0f;

            if (rotation != default)
                stampAngle = rotation.eulerAngles.z;

            D2dStamp.All(data.StampPaint, position == default ? transform.TransformPoint(transform.localPosition + _localOffset) : position,
                targetSize, stampAngle, data.StampShape, data.DestructColor, data.Mask);
        }

        public void Execute(DestructableData data, Vector3 position)
        {
            if (data.IsDestructable == false)
                return;

            float stampAngle = data.StampRandomDirection == true ? Random.Range(-180.0f, 180.0f) : 0.0f;
            D2dStamp.All(data.StampPaint, position, data.StampSize, stampAngle, data.StampShape, data.DestructColor, data.Mask);
        }
    }
}
