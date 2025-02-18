using System;
using System.Threading;
using Factories.Data;
using Factories.Infrastructure;
using Factories.View;
using Orders;
using Rails;
using Rails.Infrastructure;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Factories.Implementation
{
    public class Robot : IRobot
    {
        private RobotView _view;
        private RobotData _data;
        private CancellationTokenSource _cancellationTokenSource; // TODO: Cancel movement after collision
        private IWaypoint _nextWaypoint;

        public WorkType WorkType => _data.WorkType;
        public Vector3 Position => _view.transform.position;
        public bool IsTrackingRequired { get; set; }

        public event Action CollisionDetected;

        public event Action<IRobot> RobotDestroyRequested;

        public void Initialize(RobotView view, RobotData data)
        {
            _view = view;
            _data = data;
        }

        public async void SetNextWaypoint(IWaypoint waypoint, IIntermediateWaypoint intermediateWaypoint = null)
        {
            try
            {
                _cancellationTokenSource?.Cancel();
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

        public void Destroy()
        {
            _cancellationTokenSource?.Cancel();
            Object.Destroy(_view.gameObject);
        }
    }
}