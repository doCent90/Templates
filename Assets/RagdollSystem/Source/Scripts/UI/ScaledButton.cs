using UnityEngine;

namespace RagdollSystem
{
    public class ScaledButton : BaseButton
    {
        [SerializeField] private GameObject _visual;

        public virtual void Show()
        {
            _visual.SetActive(true);
            _visual.transform.localScale = Vector2.zero;
            _visual.transform.localScale = Vector2.one;
        }

        public virtual void Hide()
        {
            _visual.transform.localScale = Vector2.zero;
            _visual.SetActive(false);
        }
    }
}
