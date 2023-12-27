using Core;
using Destructible2D;
using System;

namespace RagdollSystem
{
    [Serializable]
    public class GameComponets
    {
        public EntitiesToggleService EntitiesToggleService;
        public ControlButtonsHandler ControlButtonsHandler;
        public JumpButton JumpButton;
        public ExitCharacterButton ExitCharacterButton;
        public CameraConverter CameraConverter;
        public IDestructionQueueHandler DestructionQueueHandler;
    }
}
