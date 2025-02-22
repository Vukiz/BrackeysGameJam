using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VFX.Implementation;

namespace Orders.Views
{
    public class ObjectView : MonoBehaviour
    {
        [SerializeField] private Outliner _outliner;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void Hide()
        {
            HideInternal().Forget();
        }

        public void AnimateOutline(float duration)
        {
            if (duration <= 0)
            {
                return;
            }
            
            _outliner.AnimateOutline(duration);
        }

        public void DisableOutline()
        {
            _outliner.DisableOutline();
        }

        private async UniTask HideInternal()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            if (!gameObject)
            {
                return;
            }
            
            gameObject.SetActive(false);
        }
    }
}