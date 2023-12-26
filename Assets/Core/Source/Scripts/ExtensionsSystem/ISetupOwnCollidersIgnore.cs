using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface ISetupOwnCollidersIgnore
    {
        void SetIgnoreOwnCollisions(IReadOnlyList<Collider2D> selfColliders, bool ignore);
    }
}
