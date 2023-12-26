using UnityEngine;

namespace RagdollSystem
{
    public class ControlButtonsHandler : MonoBehaviour, IControlButtonsHandler
    {
        [field: Header("Control buttons")]
        [field: SerializeField] public Joystick Joystick { get; private set; }
    }
}
