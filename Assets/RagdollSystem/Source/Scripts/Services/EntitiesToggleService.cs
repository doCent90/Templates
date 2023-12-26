using System.Collections.Generic;

namespace RagdollSystem
{
    public class EntitiesToggleService
    {
        private readonly ControlButtonsHandler _controlButtonsHandler;
        private readonly IDragService _dragService;
        private readonly ICharacterControllsButtons _characterControllsButtons;

        private readonly List<InputPreset> _inputPresets = new();

        public EntitiesToggleService(IDragService dragService, ICharacterControllsButtons characterControllsButtons,
            ControlButtonsHandler controlButtonsHandler)
        {
            _dragService = dragService;
            _controlButtonsHandler = controlButtonsHandler;
            _characterControllsButtons = characterControllsButtons;
        }

        public void AddPreset(InputPreset inputPreset)
        {
            _inputPresets.Add(inputPreset);

            foreach (var option in inputPreset.InputConfig.EnableOptionsList)
                EnableOption(option);

            foreach (var option in inputPreset.InputConfig.DisableOptionsList)
                DisableOption(option);
        }

        public void RemovePreset(InputPreset inputPreset)
        {
            foreach (var option in inputPreset.InputConfig.EnableOptionsList)
                DisableOption(option);

            foreach (var option in inputPreset.InputConfig.DisableOptionsList)
                EnableOption(option);

            _inputPresets.Remove(inputPreset);
        }

        private void EnableOption(Options option)
        {
            switch (option)
            {
                case Options.MoveJoystick:
                    if (_controlButtonsHandler.Joystick == null) return;
                    _controlButtonsHandler.Joystick.gameObject.SetActive(true);
                    break;
                case Options.DragAndDrop:
                    if (_dragService == null) return;
                    _dragService.EnableService();
                    break;
                case Options.ExitCharacterButton:
                    if (_characterControllsButtons.ExitCharacterButton == null) return;
                    _characterControllsButtons.ExitCharacterButton.Show();
                    break;
                case Options.JumpButton:
                    if (_characterControllsButtons.JumpButton == null) return;
                    _characterControllsButtons.JumpButton.Show();
                    break;            }
        }

        private void DisableOption(Options option)
        {
            switch (option)
            {
                case Options.MoveJoystick:
                    if (_controlButtonsHandler.Joystick == null) return;
                    _controlButtonsHandler.Joystick.gameObject.SetActive(false);
                    _controlButtonsHandler.Joystick.OnPointerUp(null);
                    break;
                case Options.DragAndDrop:
                    if (_dragService == null) return;
                    _dragService.DisableService();
                    break;
                case Options.ExitCharacterButton:
                    if (_characterControllsButtons.ExitCharacterButton == null) return;
                    _characterControllsButtons.ExitCharacterButton.Hide();
                    break;
                case Options.JumpButton:
                    if (_characterControllsButtons.JumpButton == null) return;
                    _characterControllsButtons.JumpButton.Hide();
                    break;
            }
        }
    }
}
