using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Factories.Data;
using Factories.Infrastructure;
using Factories.Views;
using Orders.Data;
using Orders.Infrastructure;
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
        private CancellationTokenSource _cancellationTokenSource;
        private IWaypoint _nextWaypoint;

        public float SelfDestructionTimerDuration { get; set; }

        private bool _isSelfDestructionTimerStarted;
        private float _speed;

        public WorkType WorkType => _view.WorkType;

        public Vector3 Position => _view.transform.position;
        public Transform Transform => _view.transform;
        public bool IsTrackingRequired { get; set; }

        public event Action<IRobot,DestroyReason> RobotDestroyRequested;

        public Robot(IVFXManager vfxManager)
        {
            _vfxManager = vfxManager;
        }
        public void Initialize(RobotView view, RobotData data)
        {
            _view = view;
            SelfDestructionTimerDuration = data.SelfDestructionTimerDuration;
            _speed = data.Speed;
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
                    duration = (intermediateWaypoint.Position - _view.transform.position).magnitude / _speed;
                    await _view.MoveTo(intermediateWaypoint.Position, duration, token);
                }

                duration = (waypoint.Position - _view.transform.position).magnitude / _speed;
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

        public async UniTask CompleteOrder(IOrder order, Vector3 sushiBeltOrderPosition)
        {
            order.ReceiveWork(WorkType);
            // move closer to the order and jump
            var sequence = DOTween.Sequence();
            var halfwayToTarget = (sushiBeltOrderPosition - Position) / 2 + Position;
            _view.LookAt(sushiBeltOrderPosition);
            sequence.Append(_view.transform.DOMove(halfwayToTarget, 0.5f));
            sequence.Append(_view.transform.DOJump(halfwayToTarget , 0.5f, 1, 0.5f));
            await sequence.Play();
            order.CheckWorkStatus();
            Debug.Log($"Robot {this} completed order {order}");
            _vfxManager.SpawnVFX(VFXType.RobotJobComplete, Position);
            RobotDestroyRequested?.Invoke(this, DestroyReason.OrderCompleted);
        }

        public void StartSelfDestructionTimer()
        {
            if (_isSelfDestructionTimerStarted || SelfDestructionTimerDuration <= 0)
            {
                return;
            }
            
            _isSelfDestructionTimerStarted = true;
            StartSelfDestructionTimerInternal().Forget();
            _view.AnimateOverheat(SelfDestructionTimerDuration);
        }

        public void StopSelfDestructionTimer()
        {
            _isSelfDestructionTimerStarted = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public async void Destroy(DestroyReason destroyReason)
        {
            if (destroyReason != DestroyReason.OrderCompleted)
            {
                // Robot jumps off the ground a little bit and lays on its back
                _view.DisableOutline();
                var sequence = DOTween.Sequence();
                sequence.Append(_view.transform.DOLocalJump(_view.transform.localPosition, 0.5f, 1, 0.5f));
                sequence.Join(_view.transform.DOLocalRotate(new Vector3(0, 0, 180), 0.5f));
                await sequence.Play();
            }

            if (!_view)
            {
                return;
            }
            
            Object.Destroy(_view.gameObject);
        }

        private async UniTask StartSelfDestructionTimerInternal()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(SelfDestructionTimerDuration));
            if (_isSelfDestructionTimerStarted && (!_cancellationTokenSource?.Token.IsCancellationRequested ?? false))
            {
                _vfxManager.SpawnVFX(VFXType.RobotSelfDestruct, Position);
                Debug.Log($"Robot {this} self-destructed.");
                RobotDestroyRequested?.Invoke(this, DestroyReason.SelfDestruction);
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