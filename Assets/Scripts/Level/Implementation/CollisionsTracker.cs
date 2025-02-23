using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Factories.Infrastructure;
using Level.Infrastructure;
using UnityEngine;
using System.Linq;

namespace Level.Implementation
{
    public class CollisionsTracker : ICollisionsTracker, IDisposable
    {
        private readonly ILevelDataModel _levelDataModel;
        private const float CollisionRadius = 0.1f;

        private readonly List<IRobot> _robots = new();
        private CancellationTokenSource _cancellationTokenSource = new();
        public event Action<IRobot> RobotCollisionDetected;

        public CollisionsTracker(
            ILevelDataModel levelDataModel
            )
        {
            _levelDataModel = levelDataModel;
        }
        public void TrackCollisions()
        {
            Reset();
            Setup();
            CheckForCollisions().Forget();
        }

        private void Setup()
        {
            foreach (var factorySlot in _levelDataModel.FactorySlots)
            {
                factorySlot.RobotReachedSlot += FinishGame;
            }

            foreach (var workstation in _levelDataModel.Workstations)
            {
                workstation.RobotReachedStationWithNoEmptySlots += FinishGame;
            }
        }

        public void Reset()
        {
            _cancellationTokenSource?.Cancel();
            foreach (var robot in _robots)
            {
                robot.IsTrackingRequired = false;
            }

            foreach (var factorySlot in _levelDataModel.FactorySlots)
            {
                factorySlot.RobotReachedSlot -= FinishGame;
            }

            foreach (var workstation in _levelDataModel.Workstations)
            {
                workstation.RobotReachedStationWithNoEmptySlots -= FinishGame;
            }

            _robots.Clear();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void RegisterRobot(IRobot robot)
        {
            robot.IsTrackingRequired = true;
            _robots.Add(robot);
        }
        
        public void RemoveRobot(IRobot robot)
        {
            robot.IsTrackingRequired = false;
            _robots.Remove(robot);
        }

        private async UniTask CheckForCollisions()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                // Get a snapshot of active robots to avoid collection modification issues
                var activeRobots = _robots.Where(r => r.IsTrackingRequired).ToList();

                // Check robot-to-robot collisions
                for (var i = 0; i < activeRobots.Count; i++)
                {
                    var robot = activeRobots[i];

                    // Check collisions with other robots
                    for (var j = i + 1; j < activeRobots.Count; j++)
                    {
                        var otherRobot = activeRobots[j];
                        if (!IsColliding(robot.Position, otherRobot.Position))
                        {
                            continue;
                        }

                        await HandleCollision(robot, otherRobot);
                        return;
                    }
                }

                await UniTask.Yield();
            }
        }

        private static bool IsColliding(Vector3 pos1, Vector3 pos2)
        {
            return (pos1 - pos2).magnitude <= CollisionRadius;
        }

        private UniTask HandleCollision(IRobot entity1, IRobot entity2)
        {
            Debug.LogWarning($"Collision detected between {entity1} and {entity2}");
            FinishGame(entity1); // Finish game on collision doesn't matter which robot collided
            return UniTask.CompletedTask;
        }

        private void FinishGame(IRobot robot)
        {
            _cancellationTokenSource?.Cancel();
            RobotCollisionDetected?.Invoke(robot);
        }
    }
}