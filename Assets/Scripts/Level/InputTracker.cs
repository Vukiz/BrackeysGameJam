using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Level
{
    public class InputTracker : IInitializable, IDisposable
    {
        private readonly CameraProvider _cameraProvider;
        
        private CancellationTokenSource _cancellationTokenSource;
        
        public void Initialize()
        {
            TrackInput().Forget();
        }
        
        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
        }

        private async UniTask TrackInput()
        {
            _cancellationTokenSource?.Cancel();
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
            if (!Physics.Raycast(ray, out var hit) || hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                return;
            }
            
            interactable.Interact();
        }
    }
}