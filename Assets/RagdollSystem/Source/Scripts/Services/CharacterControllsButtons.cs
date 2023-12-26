using System;
using UnityEngine;

namespace RagdollSystem
{
    [Serializable]
    internal class CharacterControllsButtons : ICharacterControllsButtons
    {
        [field: SerializeField] public ExitCharacterButton ExitCharacterButton { get; private set; }
        [field: SerializeField] public JumpButton JumpButton { get; private set; }
    }
}
