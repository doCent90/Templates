using UnityEngine;

namespace RagdollSystem
{
    public abstract class PuppetInputState : State
    {
        private readonly BaseCharacter _character;
        protected readonly PuppetMaster2D PuppetMaster;
        protected readonly EqupmentStateMachine StateMachine;
        protected readonly float _speed;

        protected PuppetInputState(BaseCharacter character, PuppetMaster2D puppetMaster, EqupmentStateMachine stateMachine, float speed)
        {
            _character = character;
            PuppetMaster = puppetMaster;
            StateMachine = stateMachine;
            _speed = speed;
        }

        public override void InputVelocity(Vector2 input)
        {
            base.InputVelocity(input);

            PuppetMaster.HorizontalInput = input.x * _speed;

            if (StateMachine.EquipmentState == null)
            {
                _character.RotateInDirection(input.x);
            }
        }
    }
}
