using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Factories.Data;
using Factories.Infrastructure;
using Factories.Views;
using Orders;
using Orders.Data;
using Orders.Infrastructure;
using Rails;
using Rails.Infrastructure;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Factories.Implementation
{
    public class Robot : IRobot, IDisposable
    {
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
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void Destroy()
        {
            Object.Destroy(_view.gameObject);
        }

        private async UniTask StartSelfDestructionTimerInternal()
        {
            try
            {
                var remainingTime = 0f;
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = new CancellationTokenSource();
                while (remainingTime < _data.SelfDestructionTimerDuration)
                {
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    remainingTime += Time.deltaTime;
                    await UniTask.Yield(_cancellationTokenSource.Token);
                }
                
                RobotDestroyRequested?.Invoke(this);
                Debug.Log("Robot commited self destruction");
            }
            catch (Exception e)
            {
                Debug.Log(e);
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