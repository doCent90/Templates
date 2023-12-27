using System;
using UnityEngine;
using Destructible2D;
using Object = UnityEngine.Object;
using Core;

namespace PhysicleMaterialsDamager
{
    public class DestructableSplitter
    {
        private readonly ObstacleDestructiblePart _obstacleDestructiblePart;
        private readonly D2dDestructibleSprite _destructibleSprite;
        private readonly D2dCalculateMass _calculateMass;
        private readonly D2dFracturer _fracturer;
        private readonly Rigidbody2D _selfBody;
        private readonly Action<Rigidbody2D, Collider2D> _broken;
        private readonly Collider2D _collider;
        private readonly Transform _transform;
        private readonly CameraConverter _cameraConverter;
        private readonly IDestructionQueueHandler _destructionQueueHandler;
        private readonly bool _onlyOneFracture;
        private readonly float _massToFadeOut;
        private readonly int _optimizeFactor;

        private bool _isBroken;

        public DestructableSplitter(Action<Rigidbody2D, Collider2D> broken, Collider2D collider, Transform transform, ObstacleDestructiblePart obstacleDestructiblePart, D2dFracturer fracturer, D2dDestructibleSprite destructibleSprite,
            D2dCalculateMass calculateMass, Rigidbody2D selfBody, CameraConverter cameraConverter, IDestructionQueueHandler destructionQueueHandler, float massToFadeOut, bool onlyOneFracture, int optimizeFactor)
        {
            _obstacleDestructiblePart = obstacleDestructiblePart;
            _destructionQueueHandler = destructionQueueHandler;
            _destructibleSprite = destructibleSprite;
            _onlyOneFracture = onlyOneFracture;
            _cameraConverter = cameraConverter;
            _optimizeFactor = optimizeFactor;
            _calculateMass = calculateMass;
            _massToFadeOut = massToFadeOut;
            _fracturer = fracturer;
            _broken = broken;
            _collider = collider;
            _transform = transform;
            _selfBody = selfBody;
        }

        public void Rebuild()
        {
            _destructibleSprite.Rebuild();

            for (int i = 0; i < _optimizeFactor; i++)
                _destructibleSprite.Optimize();
        }

        public void OnBroken() => _isBroken = true;

        public void OnDestructibleSplited(D2dDestructible piece)
        {
            EnvironmentObject pieceEnvironment = (EnvironmentObject)piece.entity;
            pieceEnvironment.Construct(_cameraConverter, _destructionQueueHandler);
            piece.transform.parent = _transform.parent;

            DestructibleObstacle piceDestructibleObstacle = pieceEnvironment.DestructibleObstacle;
            piceDestructibleObstacle.DestructableSplitter.OnBroken();
            pieceEnvironment.RigidBody.velocity = _selfBody.velocity / 2;
            piceDestructibleObstacle.ObstacleDestructible.Init();

            if (piceDestructibleObstacle.PhysicsMaterialsHandler != null)
            {
                piceDestructibleObstacle.PhysicsMaterialsHandler.ResetFX();
                Object.Destroy(piceDestructibleObstacle.PhysicsMaterialsHandler);
            }

            Object.Destroy(piceDestructibleObstacle.Fracturer);
            Object.Destroy(piceDestructibleObstacle.Splitter);
            Object.Destroy(piceDestructibleObstacle);

            _obstacleDestructiblePart.Init(0.2f);
        }

        public void Broke()
        {
            if (_onlyOneFracture && _isBroken)
                return;

            FakeBroke();
            _destructionQueueHandler.AddEvent(() =>
            {
                _fracturer.TryFracture();
            });

            _broken?.Invoke(_selfBody, _collider);
        }

        public void FakeBroke()
        {
            _isBroken = true;
            _calculateMass.UpdateMass();

            if (_calculateMass.LastSetMass <= _massToFadeOut)
                _obstacleDestructiblePart.Init(0.2f);
        }
    }
}
