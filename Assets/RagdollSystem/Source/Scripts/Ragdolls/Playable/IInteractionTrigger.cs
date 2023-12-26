namespace RagdollSystem
{
    public interface IInteractionTrigger
    {
        public BaseCharacter BaseCharacter { get; }
        public bool CanInteract();
    }
}