using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Factories.Data;
using Factories.Infrastructure;
using Factories.View;
using Orders;
using Rails;
using Rails.Infrastructure;
using UnityEngine;

namespace Factories.Implementation
{
    public class Factory : IFactory
    {
        private readonly RobotProvider _robotProvider;

        private FactoryView _view;
        private FactoryData _data;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isPaused;
        
        public Factory(RobotProvider robotProvider)
        {
            _robotProvider = robotProvider;
        }
        
        private IWaypoint _next;
        public Vector3 Position => _view.WaypointTransform.position;
        public WorkType WorkType => _data.WorkType;

        public event Action<IRobot> RobotSpawned;
        
        public void Initialize(FactoryView view, FactoryData data, IWaypoint next)
        {
            _view = view;
            _data = data;
            _next = next;
            StartCycle().Forget();
        }
        
        public void Reach(IRobot robot)
        {
            // TODO: Explode robot and finish the game
        }

        public void AddNeighbour(IWaypoint waypoint)
        {
            
        }

        public void PauseCycle()
        {
            _isPaused = true;
        }

        public void ResumeCycle()
        {
            _isPaused = false;
        }

        private async UniTask StartCycle()
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;
                await UniTask.Delay(TimeSpan.FromSeconds(_data.InitialDelay), cancellationToken: token);
                while (!token.IsCancellationRequested)
                {
                    SpawnRobot();
                    await StartCooldown(token);
                }
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e);
            }
        }

        private async UniTask StartCooldown(CancellationToken token)
        {
            var duration = 0f;
            while (duration <= _data.SpawnCooldown)
            {
                token.ThrowIfCancellationRequested();
                if (!_isPaused)
                {
                    duration += Time.deltaTime;
                }
                
                await UniTask.Yield();
            }
        }

        private void SpawnRobot()
        {
            var robot = _robotProvider.Create(WorkType, _view.RobotSpawnPoint.position);
            robot.SetNextWaypoint(_next);
            RobotSpawned?.Invoke(robot);
        }
    }
}