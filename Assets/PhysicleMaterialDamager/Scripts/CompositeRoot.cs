using Core;
using UnityEngine;
using Destructible2D;
using System.Collections.Generic;

namespace PhysicleMaterialsDamager
{
    public class CompositeRoot : MonoBehaviour, ICoroutine
    {
        [SerializeField] private CameraConverter _cameraConverter;
        [SerializeField] private DragAndDrop _dragAndDrop;
        private DestructionQueueHandler _destructionQueueHandler;

        [SerializeField] private List<EnvironmentObject> _environmentObjects;
        [SerializeField] private List<RangePickableWeapon> _rangePickableWeapons;

        private void Awake()
        {
            _dragAndDrop.Construct(_cameraConverter);
            _destructionQueueHandler = new DestructionQueueHandler(this);

            _environmentObjects.ForEach(envi => envi.Construct(_cameraConverter, _destructionQueueHandler));
            _rangePickableWeapons.ForEach(weapon => weapon.Construct(_cameraConverter));
            _rangePickableWeapons.ForEach(weapon => weapon.Init());
        }
    }
}
