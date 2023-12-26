using System.Collections.Generic;
using UnityEngine;

namespace RagdollSystem
{
    public interface IIgnoreColliders
    {
        IReadOnlyList<Collider2D> Colliders { get; }

        void SetIgnoreColliders();
    }
}
