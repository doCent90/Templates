using UnityEngine;

namespace RagdollSystem
{
    public class Player : BaseUnit
    {
        public void Construct() => Debug.Log("Playr inited");

        public void Setup() => Debug.Log("Playr setted");

        public void OnPlayerDie() => Debug.Log("Die");
    }
}
