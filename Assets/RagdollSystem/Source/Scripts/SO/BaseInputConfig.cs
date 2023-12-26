using System.Collections.Generic;
using UnityEngine;

namespace RagdollSystem
{
    public enum Options
    {
        MoveJoystick = 1,
        DragAndDrop = 2,
        ExitCharacterButton = 3,
        JumpButton = 4,
    }

    [CreateAssetMenu(menuName = "SO/InputConfig", fileName = "InputConfig")]
    public class BaseInputConfig : ScriptableObject
    {
        public List<Options> EnableOptionsList;
        public List<Options> DisableOptionsList;
    }
}