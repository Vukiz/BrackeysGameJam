using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Views
{
    public class StarsContainerView : MonoBehaviour
    {
        [SerializeField] private Image _star1;
        [SerializeField] private Image _star2;
        [SerializeField] private Image _star3;

        private CancellationTokenSource _cancellationTokenSource;

        public async UniTask PlayStarsAnimation(int reachedStars)
        {
            _star1.fillAmount = 0;
            _star2.fillAmount = 0;
            _star3.fillAmount = 0;
            _star1.gameObject.SetActive(true);
            _star2.gameObject.SetActive(true);
            _star3.gameObject.SetActive(true);

            // Fill up the stars one by one with slight delay between each reached star using dotween
            for (var i = 0; i < reachedStars; i++)
            {
                switch (i)
                {
                    case 0:
                        await _star1.DOFillAmount(1, 0.5f);
                        break;
                    case 1:
                        await _star2.DOFillAmount(1, 0.5f);
                        break;
                    case 2:
                        await _star3.DOFillAmount(1, 0.5f);
                        break;
                }
                
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            }
        }
    }
}