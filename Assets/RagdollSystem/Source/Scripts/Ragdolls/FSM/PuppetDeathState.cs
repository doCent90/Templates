namespace RagdollSystem
{
    public class PuppetDeathState : State
    {
        private readonly StandingSystem _standingSystem;

        public PuppetDeathState(StandingSystem standingSystem) 
            => _standingSystem = standingSystem;

        public override void Enter()
        {
            base.Enter();
            _standingSystem.MakeDead();
        }
    }
}
