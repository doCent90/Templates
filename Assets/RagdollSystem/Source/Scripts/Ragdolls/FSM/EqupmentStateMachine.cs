namespace RagdollSystem
{
    public class EqupmentStateMachine
    {
        public EquipmentState EquipmentState { get; set; }
        public State State { get; set; }

        public void InitializeEquipment(EquipmentState startState)
        {
            EquipmentState = startState;
            EquipmentState.Enter();
        }

        public void InitializeControl(State startState)
        {
            State = startState;
            State.Enter();
        }

        public void ChangeEquipmentState(EquipmentState newState)
        {
            EquipmentState?.Exit();
            EquipmentState = newState;
            EquipmentState?.Enter();
        }

        public void ChangeState(State newState)
        {
            State?.Exit();
            State = newState;
            State?.Enter();
        }
    }
}
