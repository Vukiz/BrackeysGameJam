using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Factories.Implementation;
using Factories.Infrastructure;
using Level.Infrastructure;
using StateMachine.Data;
using StateMachine.Infrastructure;
using UnityEngine;

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
            _cancellationTokenSource?.Dispose();
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
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                for (var i = 0; i < _factorySlots.Count; i++)
                {
                    var left = _robots[i];
                    if (!left.IsTrackingRequired)
                    {
                        continue;
                    }
                    
                    for (var j = i; j < _factorySlots.Count; j++)
                    {
                        var right = _robots[j];
                        if (i == j || !right.IsTrackingRequired)
                        {
                            continue;
                        }
                        
                        if ((left.Position - right.Position).magnitude <= CollisionRadius)
                        {
                            FinishGame();
                            return;
                        }
                    }
                }

                await UniTask.Yield();
            }
        }

        private void FinishGame()
        {
            // TODO: End game
            Debug.Log("Game ended because of robot collisions");
            RobotCollisionDetected?.Invoke();
        }
    }
}