using UnityEngine;

namespace RagdollSystem
{
    public abstract class BaseCharacterControll : MonoBehaviour
    {
        protected IInputListener InputListener;

        public virtual void Init() => InputListener = GetComponentInChildren<IInputListener>();
        public abstract void OnEnter();
        public abstract void OnExit();
    }
}
