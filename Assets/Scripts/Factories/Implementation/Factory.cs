using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Factories.Data;
using Factories.Infrastructure;
using Factories.Views;
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
        private static readonly int OpenDoors = Animator.StringToHash("OpenDoors");
        public float SpawnProgress => _timeSinceLastSpawn / _data.SpawnCooldown;
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
            _view.SignalGlow.Initialize(this);
            _view.Destroyed += Dispose;
            _view.LookAt(next.Position);
        }

        public void SetPaused(bool isPaused)
        {
            _isPaused = isPaused;
        }

        public async UniTask StartCycle()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            await UniTask.Delay(TimeSpan.FromSeconds(_data.InitialDelay), cancellationToken: token);
            _timeSinceLastSpawn = _data.SpawnCooldown;
            while (!token.IsCancellationRequested)
            {
                if (!_isPaused)
                {
                    _timeSinceLastSpawn += Time.deltaTime;
                    if (_timeSinceLastSpawn >= _data.SpawnCooldown)
                    {
                        _timeSinceLastSpawn = 0;
                        SpawnRobot().Forget();
                    }
                }

                await UniTask.Yield();
            }
        }

        public async UniTask<IRobot> SpawnRobot()
        {
            var (robot, robotView) = _robotProvider.Create(_data.WorkType, _view.RobotSpawnPoint.position, _robotsParent);
            _view.DoorsAnimator.SetTrigger(OpenDoors);
            await robotView.transform.DOJump(_view.RobotLaunchPoint.position, 2, 1, 1)
                .SetEase(_view.LaunchCurve)
                .WithCancellation(_cancellationTokenSource.Token);
            robot.SetNextWaypoint(_next);
            return robot;
        }

        public void Dispose()
        {
            _view.Destroyed -= Dispose;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}