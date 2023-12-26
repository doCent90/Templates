using UnityEngine;

namespace RagdollSystem
{
    public class PlayerCharacterControll : BaseCharacterControll
    {
        protected InputPreset CharacterPreset;
        protected InputPreset EquipmentPreset;
        protected bool AttackEnable;
        protected bool BlockInput = false;

        private BaseInputConfig _baseInputConfig;
        private JumpButton _jumpButton;
        private Joystick _movementJoystick;
        private EntitiesToggleService _entitiesToggleService;

        [field: SerializeField] public Player Player { get; protected set; }

        private void Update() => UpdateInput();

        public void Construct(GameComponets gameComponets)
        {
            Player.Construct();
            _jumpButton = gameComponets.JumpButton;
            _entitiesToggleService = gameComponets.EntitiesToggleService;
            _movementJoystick = gameComponets.ControlButtonsHandler.Joystick;

            base.Init();
        }

        public void Setup(BaseInputConfig inputConfig, IInputListener inputListener)
        {
            InputListener = inputListener;
            _baseInputConfig = inputConfig;
        }

        private void ZeroInput() => InputListener.InputMovement(Vector2.zero);

        protected virtual void UpdateInput()
        {
            if (BlockInput) return;
            Vector2 movementVector = new Vector2(0, 0);

            if (Input.GetKey(KeyCode.D)) movementVector += new Vector2(1, 0);
            if (Input.GetKey(KeyCode.A)) movementVector += new Vector2(-1, 0);
            if (Input.GetKey(KeyCode.S)) movementVector += new Vector2(0, -1);
            if (Input.GetKey(KeyCode.W)) movementVector += new Vector2(0, 1);
            if (Input.GetKeyDown(KeyCode.Space)) OnJumpClick();

            if (movementVector.magnitude == 0)
                InputListener.InputMovement(_movementJoystick.Direction);
            else
                InputListener.InputMovement(movementVector);

            if (EquipmentPreset != null)
                Debug.Log("Equipmented");
        }

        private void OnJumpClick() => InputListener.Jump();

        public override void OnEnter()
        {
            AttackEnable = false;

            CharacterPreset = new InputPreset(_baseInputConfig);
            _entitiesToggleService.AddPreset(CharacterPreset);

            InputListener.SetUnderPlayerControll(true);
            _jumpButton.OnClick += OnJumpClick;
        }

        public override void OnExit()
        {
            ZeroInput();
            InputListener.SetUnderPlayerControll(false);

            _jumpButton.OnClick -= OnJumpClick;
            _entitiesToggleService.RemovePreset(CharacterPreset);
        }
    }
}
