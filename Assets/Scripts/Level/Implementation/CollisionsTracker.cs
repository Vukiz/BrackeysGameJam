using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Factories.Implementation;
using Factories.Infrastructure;
using Level.Infrastructure;
using UnityEngine;
using System.Linq;

namespace Level.Implementation
{
    public class CollisionsTracker : ICollisionsTracker, IDisposable
    {
        private const float CollisionRadius = 0.1f;
        
        private readonly List<FactorySlot> _factorySlots = new();
        private readonly List<IRobot> _robots = new();
        private CancellationTokenSource _cancellationTokenSource;

        public void TrackCollisions()
        {
            CheckForCollisions().Forget();
        }

        public void Reset()
        {
            _cancellationTokenSource?.Cancel();
            _factorySlots.Clear();
            _robots.Clear();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        public void RegisterFactorySlot(FactorySlot factorySlot)
        {
            _factorySlots.Add(factorySlot);
            factorySlot.RobotReachedSlot += FinishGame;
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

        public event Action RobotCollisionDetected;

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
                    //
                    // // Check collisions with factories
                    // foreach (var factory in _factorySlots.Where(factory => IsColliding(robot.Position, factory.Position)))
                    // {
                    //     await HandleCollision(robot, factory);
                    //     return;
                    // }
                    // TODO It doesn't work because robot immediately collides with a factory on spawn
                }

                await UniTask.Yield();
            }
        }

        private bool IsColliding(Vector3 pos1, Vector3 pos2)
        {
            return (pos1 - pos2).magnitude <= CollisionRadius;
        }

        private UniTask HandleCollision(object entity1, object entity2)
        {
            Debug.LogWarning($"Collision detected between {entity1} and {entity2}");
            FinishGame();
            return UniTask.CompletedTask;
        }

        private void FinishGame()
        {
            // TODO: End game
            Debug.Log("Game ended because of robot collisions");
            RobotCollisionDetected?.Invoke();
        }
    }
}