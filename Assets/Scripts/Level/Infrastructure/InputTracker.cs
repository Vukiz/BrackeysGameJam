using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Level.Implementation;
using UnityEngine;
using Zenject;

namespace Level.Infrastructure
{
    public class InputTracker : IInitializable, IDisposable
    {
        private readonly CameraProvider _cameraProvider;

        private CancellationTokenSource _cancellationTokenSource;

        public InputTracker(CameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
        }

        public void Initialize()
        {
            TrackInput().Forget();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async UniTask TrackInput()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    HandleInput();
                }

                await UniTask.Yield();
            }
        }

        private void HandleInput()
        {
            var ray = _cameraProvider.MainCamera.ScreenPointToRay(Input.mousePosition);
            // if (!Physics.Raycast(ray, out var hit, 100f, 1 << 12)) // TODO: use layers
            if (!Physics.Raycast(ray, out var hit))
            {
                return;
            }

            if (!hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                return;
            }

            interactable.Interact();
        }
    }
}