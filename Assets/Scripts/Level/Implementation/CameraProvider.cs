using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Level.Implementation
{
    public class CameraProvider
    {
        public Camera MainCamera { get; }
        private readonly Vector3 _originalPosition;
        private readonly float _originalSize;
        private const float MovementDuration = 0.5f;

        public CameraProvider(Camera camera)
        {
            MainCamera = camera;
            _originalPosition = camera.transform.position;
            _originalSize = camera.orthographicSize;
        }

        public async UniTask FocusOn(Vector3 target, float zoomSize = 3f)
        {
            Vector3 targetPosition = new(target.x, target.y, _originalPosition.z);

            var sequence = DOTween.Sequence();
            sequence.Join(MainCamera.transform.DOMove(targetPosition, MovementDuration));
            sequence.Join(MainCamera.DOOrthoSize(zoomSize, MovementDuration));

            await sequence.Play();
        }

        public async UniTask ResetToOriginalPosition()
        {
            var sequence = DOTween.Sequence();
            sequence.Join(MainCamera.transform.DOMove(_originalPosition, MovementDuration));
            sequence.Join(MainCamera.DOOrthoSize(_originalSize, MovementDuration));

            await sequence.Play();
        }
    }
}