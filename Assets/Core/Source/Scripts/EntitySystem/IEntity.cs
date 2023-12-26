using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface IEntity
    {
        IReadOnlyList<Transform> AllParts { get; }
        Transform Transform { get; }
    }
}
