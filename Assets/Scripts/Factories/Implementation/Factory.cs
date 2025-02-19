using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Factories.Data;
using Factories.Infrastructure;
using Factories.View;
using Orders.Data;
using Rails.Infrastructure;
using UnityEngine;
using IFactory = Factories.Infrastructure.IFactory;

namespace Factories.Implementation
{
    public class Factory : IFactory, IDisposable
    {
        private readonly RobotProvider _robotProvider;
        private bool _isPaused;
        private float _timeSinceLastSpawn;

        private FactoryView _view;
        private FactoryData _data;
        private Transform _robotsParent;
        private CancellationTokenSource _cancellationTokenSource = new();
        private IWaypoint _next;
        public event Action<IRobot> RobotSpawned;
        public WorkType WorkType => _data.WorkType;

        public Factory(RobotProvider robotProvider)
        {
            _robotProvider = robotProvider;
        }

        public void Initialize(FactoryView view, FactoryData data, IWaypoint next, Transform robotsParent)
        {
            _view = view;
            _data = data;
            _next = next;
            _isPaused = false;
            _robotsParent = robotsParent;
            StartCycle().Forget();
        }

        public void SetPaused(bool isPaused)
        {
            _isPaused = isPaused;
        }

        private async UniTask StartCycle()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            await UniTask.Delay(TimeSpan.FromSeconds(_data.InitialDelay), cancellationToken: token);
            _timeSinceLastSpawn = _data.SpawnCooldown;
            Debug.Log($"Factory {WorkType} started");
            while (!token.IsCancellationRequested)
            {
                if (!_isPaused)
                {
                    _timeSinceLastSpawn += Time.deltaTime;
                    if (_timeSinceLastSpawn >= _data.SpawnCooldown)
                    {
                        _timeSinceLastSpawn = 0;
                        SpawnRobot();
                    }
                }

                await UniTask.Yield();
            }
        }

        private void SpawnRobot()
        {
            var robot = _robotProvider.Create(WorkType, _view.RobotSpawnPoint.position, _robotsParent);
            robot.SetNextWaypoint(_next);
            RobotSpawned?.Invoke(robot);
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}