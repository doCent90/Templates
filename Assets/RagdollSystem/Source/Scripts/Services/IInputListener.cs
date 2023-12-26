using System;
using UnityEngine;

namespace RagdollSystem
{
    public interface IInputListener
    {
        event Action<BaseCharacter> OnDie;

        void SetUnderPlayerControll(bool value);
        void InputMovement(Vector2 direction);
        void LookAt(float value);
        void Jump();
    }
}
