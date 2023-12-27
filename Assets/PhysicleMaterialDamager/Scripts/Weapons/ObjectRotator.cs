using UnityEngine;

namespace PhysicleMaterialsDamager
{
    public class ObjectRotator : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private LookSide _defaultLookSide;
        [SerializeField] private LookSide _currentLookSide;

        public LookSide DefaultLookSide => _defaultLookSide;
        public LookSide LookSide => _currentLookSide;

        public void Init()
        {
            _currentLookSide = _defaultLookSide;

            if (_target == null)
                _target = transform;
        }

        public void SetupRotation(LookSide lookSide) => _currentLookSide = lookSide;

        public bool Rotate(LookSide lookSide)
        {
            if (lookSide == _currentLookSide) return false;
            _currentLookSide = lookSide;

            switch (_currentLookSide)
            {
                case LookSide.Left:
                    _target.transform.localScale = new Vector3(-_target.transform.localScale.x,
                        _target.transform.localScale.y, _target.transform.localScale.z);
                    break;
                case LookSide.Right:
                    _target.transform.localScale = new Vector3(-_target.transform.localScale.x,
                        _target.transform.localScale.y, _target.transform.localScale.z);
                    break;
            }

            return true;
        }
    }

    public enum LookSide
    {
        Left,
        Right,
    }
}
