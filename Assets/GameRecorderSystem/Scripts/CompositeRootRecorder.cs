using Extensions;
using System.Collections.Generic;
using UnityEngine;

namespace GameRecorder
{
    public class CompositeRootRecorder : MonoBehaviour, ICoroutine
    {
        [SerializeField] private List<Entity> _entities;
        [SerializeField] private TicksHandler _ticksHandler;
        [SerializeField] private RecorderHUDHandler _recorderHUDHandler;

        private GameRecorder _gameRecorder;

        public IReadOnlyList<Entity> Entities => _entities;

        private void Awake() => Construct();

        private void OnDestroy()
        {
            if(_gameRecorder == null) return;

            _gameRecorder.Dispose();
        }

        public void Construct()
        {
            _entities.ForEach(entity => entity.Construct());

            _ticksHandler.Construct(this);
            _gameRecorder = new GameRecorder(_entities, _ticksHandler, this);
            _recorderHUDHandler.Construct(_gameRecorder);
        }
    }
}
