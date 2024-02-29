using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace GameRecorder
{
    [Serializable]
    public class GameRecorder : IGameRecorder
    {
        private readonly IReadOnlyList<Entity> _entities;
        private readonly ITicksHandler _tickHandler;
        private readonly ICoroutine _coroutine;
        private readonly List<PhysicleData[]> _frameDatas = new();
        private PhysicleData[] _lastPhysStateData;

        private Coroutine _current;
        private int _lastPlayedIndex = 0;
        private int _lastManualControlledIndex = 0;
        private bool _isManualControl = false;

        public IReadOnlyList<PhysicleData[]> FramesData => _frameDatas;
        public int CurrentFrameIndex { get; private set; } = 0;

        public GameRecorder(IReadOnlyList<Entity> entities, ITicksHandler ticksHandler, ICoroutine coroutine)
        {
            _entities = entities;
            _coroutine = coroutine;
            _tickHandler = ticksHandler;

            _tickHandler.OnTick += Record;
        }
        public void Dispose() => _tickHandler.OnTick -= Record;

        public void StopRecoding()
        {
            _tickHandler.OnTick -= Record;

            SetLastStates();

            foreach (var item in _entities)
                item.StopSimulation();
        }

        public void StartPlayMode(Action onStoped = null)
        {
            _current = _coroutine.StartCoroutine(Playing());

            IEnumerator Playing()
            {
                var wait = new WaitForSeconds(_tickHandler.TickRate);

                for (int frameIndex = 0; frameIndex < _frameDatas.Count; frameIndex++)
                {
                    if (_isManualControl)
                        frameIndex = _lastManualControlledIndex;

                    if(frameIndex > _frameDatas.Count - 1)
                        frameIndex = _frameDatas.Count - 1;

                    PhysicleData[] frame = _frameDatas[frameIndex];

                    for (int i = 0; i < frame.Length; i++)
                        _entities[i].SetPositionsOnPlayMode(frame[i]);

                    _lastPlayedIndex = frameIndex;
                    CurrentFrameIndex = frameIndex;
                    yield return wait;
                }

                _current = null;
                StopPlayMode();
                onStoped?.Invoke();
            }
        }

        public void ManualPlay(int frameIndex)
        {
            _isManualControl = true;
            _lastManualControlledIndex = frameIndex;
        }

        public void ManualCancel() => _isManualControl = false;

        public void StopPlayMode(bool manualStop = false)
        {
            if (_current != null)
            {
                _coroutine.StopCoroutine(_current);
                _current = null;
            }

            if (manualStop == false)
            {
                _lastPlayedIndex = 0;
            }
            else
            {
                var frames = _frameDatas.ToArray();
                _frameDatas.Clear();

                for (int i = 0; i < _lastPlayedIndex; i++)
                    _frameDatas.Add(frames[i]);

                SetLastStates();
            }

            for (int i = 0; i < _entities.Count; i++)
                _entities[i].ContinueSimulation(_lastPhysStateData[i]);

            _tickHandler.OnTick += Record;
        }

        private void Record(ulong tick)
        {
            PhysicleData[] entitiesData = new PhysicleData[_entities.Count];

            for (int i = 0; i < _entities.Count; i++)
            {
                var data = _entities[i].Data;
                data.Ticks = tick;
                entitiesData[i] = data;
            }

            _frameDatas.Add(entitiesData);
        }

        private void SetLastStates()
        {
            _lastPhysStateData = new PhysicleData[_entities.Count];
            _lastPhysStateData = _frameDatas.LastOrDefault();
        }
    }
}
