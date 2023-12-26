using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        public Transform Transform => transform;
        public IReadOnlyList<Transform> AllParts { get; protected set; }
    }
}
