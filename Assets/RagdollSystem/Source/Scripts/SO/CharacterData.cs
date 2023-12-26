using UnityEngine;

namespace RagdollSystem
{
    [CreateAssetMenu(fileName = "New Character Data", menuName = "ScriptableObjects/CharacterData", order = 2)]
    public class CharacterData : BaseData
    {
        [field: SerializeField] public int Health;
        [field: SerializeField] public float Speed;
        [field: SerializeField] public float StayStateEnterDelay;
    }
}
