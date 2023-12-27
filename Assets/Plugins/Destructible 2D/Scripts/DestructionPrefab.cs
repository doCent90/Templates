using UnityEngine;
using Destructible2D.Examples;
using System.Collections.Generic;

namespace Assets.Plugins.Destructible2D.Required.Scripts
{
    public class DestructionPrefab : MonoBehaviour
    {
        [SerializeField] private List<D2dExplosion> _destructions;

        public void SetSize(Vector2 size)
            => _destructions.ForEach(destruction => destruction.StampSize *= size);
    }
}
