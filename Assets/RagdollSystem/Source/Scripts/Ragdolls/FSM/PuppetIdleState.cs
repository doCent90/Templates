namespace RagdollSystem
{
    public class PuppetIdleState : State
    {
        private readonly CharacterAnimator _characterAnimator;

        public PuppetIdleState(CharacterAnimator characterAnimator) 
            => _characterAnimator = characterAnimator;

        public override void Enter() 
            => _characterAnimator.SetAnimation(CharacterAnimationState.Movement);
    }
}
