using UnityEngine;

namespace Core
{
    public interface IPushable
    {
        void Push(Rigidbody2D rigidbody2D, Vector2 direction, float force);
    }
}
