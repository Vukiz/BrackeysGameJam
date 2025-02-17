using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Views
{
    public class LoaderView : BaseView
    {
        [SerializeField] private float _animationDuration = 1f;
        [SerializeField] private Image _loaderImage;

        private CancellationTokenSource _cancellationTokenSource = new();

        public override async UniTask Show()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            _loaderImage.fillAmount = 0;
            await _loaderImage.DOFillAmount(1, _animationDuration)
                .SetEase(Ease.Linear)
                .WithCancellation(_cancellationTokenSource.Token);
            await base.Show();
        }

        public override async UniTask Hide()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            await _loaderImage.DOFillAmount(0, _animationDuration)
                .SetEase(Ease.Linear).WithCancellation(_cancellationTokenSource.Token);
            await base.Hide();
        }
    }
}