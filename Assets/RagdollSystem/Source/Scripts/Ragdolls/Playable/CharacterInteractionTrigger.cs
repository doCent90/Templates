using UnityEngine;

namespace RagdollSystem
{
    public class CharacterInteractionTrigger : MonoBehaviour, IInteractionTrigger
    {
        [SerializeField] private BaseCharacter _baseCharacter;

        public BaseCharacter BaseCharacter => _baseCharacter;

        public bool CanInteract() => _baseCharacter.CanInteract();
    }
}
