using UnityEngine;

namespace Destructible2D
{
    [CreateAssetMenu(menuName = "SO/DamagableData", fileName = "DamagableData")]
    public class DestructableData : ScriptableObject
    {
        public bool IsDestructable;
        public Color DestructColor;
        public LayerMask Mask;
        public D2dDestructible.PaintType StampPaint;
        public int Priority;
        public Vector2 StampSize;
        public bool StampRandomDirection = true;
        public Texture2D StampShape;
    }
}
