using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VFX.Implementation
{
    public class Outliner : MonoBehaviour
    {
        [SerializeField] private Outline _outline;

        private bool _isOutlineEnabled;

        public void AnimateOutline(float duration, float initialInterval = 1f, bool shouldDecreaseInterval = true, bool shouldAnimateScale = true)
        {
            if (_isOutlineEnabled)
            {
                return;
            }
            
            _isOutlineEnabled = true;
            AnimateOutlineInternal(duration, initialInterval, shouldDecreaseInterval, shouldAnimateScale).Forget();
        }
        
        public void DisableOutline()
        {
            var color = _outline.OutlineColor;
            color.a = 0f;
            _outline.OutlineColor = color;
            _outline.enabled = false;
        }

        private async UniTask AnimateOutlineInternal(float duration, float initialInterval, bool shouldDecreaseInterval, bool shouldAnimateScale)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration / 2));
            var interval = initialInterval;
            var delta = initialInterval / duration * 2f;
            while (delta > 0 && _outline && _outline.enabled)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(interval));
                Highlight(true, shouldAnimateScale);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
                Highlight(false, shouldAnimateScale);
                if (shouldDecreaseInterval)
                {
                    interval = Mathf.Clamp(interval - delta, 0.1f, 1f);
                }
            }
        }
        
        private void Highlight(bool isHighlighted, bool shouldAnimateScale)
        {
            if (!_outline)
            {
                return;
            }
            
            var alpha = isHighlighted ? 1f : 0f;
            var color = _outline.OutlineColor;
            color.a = alpha;
            _outline.OutlineColor = color;
            if (shouldAnimateScale)
            {
                var scaleMultiplier = isHighlighted ? 1.1f : 1f;
                transform.localScale = Vector3.one * scaleMultiplier;
            }
        }
    }
}