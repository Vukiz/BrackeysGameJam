using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;

namespace Level.Implementation
{
    public class CameraProvider
    {
        public Camera MainCamera { get; }
        private readonly Vector3 _originalPosition;
        private readonly float _originalSize;
        private const float MovementDuration = 0.5f;
        private const float CameraDistance = 30f;
        
        private CancellationTokenSource _trackingCts;
        private bool _isTracking;
        private Transform _trackedTarget;

        public CameraProvider(Camera camera)
        {
            MainCamera = camera;
            _originalPosition = camera.transform.position;
            _originalSize = camera.orthographicSize;
        }

        public async UniTask FocusOn(Vector3 target, float zoomSize = 3f)
        {
            // align with target position but move backwards alongside camera forward vector
            var targetPosition = target - MainCamera.transform.forward * CameraDistance;
            
            var sequence = DOTween.Sequence();
            sequence.Join(MainCamera.transform.DOMove(targetPosition, MovementDuration));
            sequence.Join(MainCamera.DOOrthoSize(zoomSize, MovementDuration));
            
            await sequence.Play().ToUniTask();
        }

        public async UniTask ResetToOriginalPosition()
        {
            StopTracking();
            
            var sequence = DOTween.Sequence();
            sequence.Join(MainCamera.transform.DOMove(_originalPosition, MovementDuration));
            sequence.Join(MainCamera.DOOrthoSize(_originalSize, MovementDuration));
            
            await sequence.Play().ToUniTask();
        }

        public async UniTask StartTracking(Transform target)
        {
            StopTracking();
            
            _trackingCts = new CancellationTokenSource();
            _trackedTarget = target;
            _isTracking = true;

            try
            {
                while (!_trackingCts.Token.IsCancellationRequested)
                {
                    if (_trackedTarget == null)
                    {
                        StopTracking();
                        return;
                    }

                    // Calculate target position while maintaining the same distance
                    var targetPosition = _trackedTarget.position - MainCamera.transform.forward * CameraDistance;
                    
                    // Smoothly move towards the target position
                    MainCamera.transform.position = Vector3.Lerp(
                        MainCamera.transform.position,
                        targetPosition,
                        Time.deltaTime * 5f // Adjust this multiplier to control smoothing speed
                    );
                    
                    await UniTask.Yield(PlayerLoopTiming.LastUpdate, _trackingCts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // Tracking was cancelled, this is expected
            }
            finally
            {
                _isTracking = false;
            }
        }

        public void StopTracking()
        {
            if (!_isTracking)
            {
                return;
            }

            _trackingCts?.Cancel();
            _trackingCts?.Dispose();
            _trackingCts = null;
            _trackedTarget = null;
            _isTracking = false;
        }

        public bool IsTracking => _isTracking;
    }
}