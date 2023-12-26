using System.Collections.Generic;
using System.Linq;
using Puppet2D;
using UnityEngine;

namespace RagdollSystem
{
    public class CharacterVisual : MonoBehaviour
    {
        [SerializeField] private Puppet2D_GlobalControl _globalControl;
        [SerializeField] private List<SpriteRenderer> _visuals;

        [ContextMenu(nameof(ResetVisuals))]
        public void ResetVisuals(Transform target)
        {
            _visuals = target.GetComponentsInChildren<SpriteRenderer>().ToList();
        }

        [ContextMenu(nameof(Show))]
        public void Show()
        {
            _visuals.ForEach(item => item.enabled = true);

            _globalControl.BonesVisiblity = true;
            _globalControl.ControlsVisiblity = true;
            _globalControl.UpdateVisibility();
        }

        [ContextMenu(nameof(Hide))]
        public void Hide()
        {
            _visuals.ForEach(item => item.enabled = false);
            _globalControl.BonesVisiblity = false;
            _globalControl.ControlsVisiblity = false;
            _globalControl.UpdateVisibility();
        }
    }
}