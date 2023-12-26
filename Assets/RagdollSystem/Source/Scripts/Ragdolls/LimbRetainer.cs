using System.Collections.Generic;
using UnityEngine;

namespace Unit.Puppet
{
    public class LimbRetainer : MonoBehaviour
    {
        [SerializeField] private Transform _defaultTransform;
        [SerializeField] private Transform _bodyUp;
        [SerializeField] private Transform _bodyDown;
        [SerializeField] private List<Rigidbody2D> _arms;
        [SerializeField] private List<Rigidbody2D> _legs;

        public void Execute()
        {
            for (int i = 0; i < _legs.Count; i++)
            {
                _legs[i].simulated = false;
                _legs[i].transform.SetParent(_bodyDown);
            }
        }

        public void Cancel()
        {
            for (int i = 0; i < _legs.Count; i++)
            {
                _legs[i].simulated = true;
                _legs[i].transform.SetParent(_defaultTransform);
            }
        }
    }
}
