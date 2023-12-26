using UnityEngine;

namespace RagdollSystem
{
    public interface ICharacter
    {
        public bool OnTryBeginDrag(Rigidbody2D rigidbody);
        public void OnDrop();
    }
}
