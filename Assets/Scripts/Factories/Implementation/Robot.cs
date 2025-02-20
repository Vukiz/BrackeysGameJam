using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Factories.Data;
using Factories.Infrastructure;
using Factories.Views;
using Orders;
using Orders.Data;
using Orders.Infrastructure;
using Rails;
using Rails.Infrastructure;
using UnityEngine;
using VFX.Data;
using VFX.Infrastructure;
using Object = UnityEngine.Object;

namespace Factories.Implementation
{
    public class Robot : IRobot, IDisposable
    {
        private readonly IVFXManager _vfxManager;
        private RobotView _view;
        private RobotData _data;
        private CancellationTokenSource _cancellationTokenSource;
        private IWaypoint _nextWaypoint;
        private bool _isSelfDestructionTimerStarted;

        public WorkType WorkType => _data.WorkType;
        public Vector3 Position => _view.transform.position;
        public bool IsTrackingRequired { get; set; }

        public event Action CollisionDetected;

        public event Action<IRobot> RobotDestroyRequested;

        public Robot(IVFXManager vfxManager)
        {
            _vfxManager = vfxManager;
        }
        public void Initialize(RobotView view, RobotData data)
        {
            _view = view;
            _data = data;
            _view.Destroyed += Dispose;
        }

        public async void SetNextWaypoint(IWaypoint waypoint, IIntermediateWaypoint intermediateWaypoint = null)
        {
            try
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                float duration;
                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;
                if (intermediateWaypoint != null)
                {
                    duration = (intermediateWaypoint.Position - _view.transform.position).magnitude / _data.Speed;
                    await _view.MoveTo(intermediateWaypoint.Position, duration, token);
                }

                duration = (waypoint.Position - _view.transform.position).magnitude / _data.Speed;
                await _view.MoveTo(waypoint.Position, duration, token);
                waypoint.Reach(this);
            }
            catch (OperationCanceledException e)
            {
                Debug.Log(e);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void CompleteOrder(IOrder order)
        {
            order.ReceiveWork(WorkType);
            Debug.Log($"Robot {this} completed order {order}");
            _vfxManager.SpawnVFX(VFXType.RobotJobComplete, Position);
            RobotDestroyRequested?.Invoke(this);
        }

        public void StartSelfDestructionTimer()
        {
            if (_isSelfDestructionTimerStarted)
            {
                return;
            }
            
            _isSelfDestructionTimerStarted = true;
            StartSelfDestructionTimerInternal().Forget();
        }

        public void StopSelfDestructionTimer()
        {
            _isSelfDestructionTimerStarted = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public async void Destroy()
        {
            // Robot jumps off the ground a little bit and lays on its back
            var sequence = DOTween.Sequence();
            sequence.Append(_view.transform.DOLocalJump(_view.transform.localPosition, 0.5f, 1, 0.5f));
            sequence.Join(_view.transform.DOLocalRotate(new Vector3(0, 0, 180), 0.5f));
            await sequence.Play();
            Object.Destroy(_view.gameObject);
        }

        private async UniTask StartSelfDestructionTimerInternal()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_data.SelfDestructionTimerDuration));
            if (_isSelfDestructionTimerStarted && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                _vfxManager.SpawnVFX(VFXType.RobotSelfDestruct, Position);
                RobotDestroyRequested?.Invoke(this);
            }
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