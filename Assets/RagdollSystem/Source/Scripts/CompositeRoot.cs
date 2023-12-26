using Core;
using UnityEngine;

namespace RagdollSystem
{
    public class CompositeRoot : MonoBehaviour, ICoroutine
    {
        [SerializeField] private CharacterPresenter _characterPresenter;
        [SerializeField] private CameraConverter _cameraConverter;
        [SerializeField] private ControlButtonsHandler _controlButtonsHandler;
        [SerializeField] private DragAndDrop _dragAndDrop;
        [SerializeField] private CharacterControllsButtons _characterControllsButtons;
        [SerializeField] private BaseInputConfig _default;

        private void Awake()
        {
            _dragAndDrop.Construct(_cameraConverter);

            GameComponets gameComponets = new()
            {
                CameraConverter = _cameraConverter,
                DestructionQueueHandler = new DestructionQueueHandler(this),
                ControlButtonsHandler = _controlButtonsHandler,
                EntitiesToggleService = new EntitiesToggleService(_dragAndDrop, _characterControllsButtons, _controlButtonsHandler),
                ExitCharacterButton = _characterControllsButtons.ExitCharacterButton,
                JumpButton = _characterControllsButtons.JumpButton,
            };

            gameComponets.EntitiesToggleService.RemovePreset(new InputPreset(_default));
            _characterPresenter.Construct(gameComponets);
        }
    }
}
