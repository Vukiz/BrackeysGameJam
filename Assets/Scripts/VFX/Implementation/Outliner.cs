using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VFX.Implementation
{
    public class Outliner : MonoBehaviour
    {
        [SerializeField] private Outline _outline;

        private bool _isOutlineEnabled;

        public void AnimateOutline(float duration)
        {
            if (_isOutlineEnabled)
            {
                return;
            }
            
            _isOutlineEnabled = true;
            AnimateOutlineInternal(duration).Forget();
        }
        
        public void DisableOutline()
        {
            var color = _outline.OutlineColor;
            color.a = 0f;
            _outline.OutlineColor = color;
            _outline.enabled = false;
        }

        private async UniTask AnimateOutlineInternal(float duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(duration / 2));
            var delay = 1f;
            var delta = delay / duration * 2f;
            while (delta > 0 && _outline && _outline.enabled)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delay));
                Highlight(true);
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
                Highlight(false);
                delay = Mathf.Clamp(delay - delta, 0.1f, 1f);
            }
        }
        
        private void Highlight(bool isHighlighted)
        {
            if (!_outline)
            {
                return;
            }
            
            var alpha = isHighlighted ? 1f : 0f;
            var scaleMultiplier = isHighlighted ? 1.1f : 1f;
            var color = _outline.OutlineColor;
            color.a = alpha;
            _outline.OutlineColor = color;
            transform.localScale = Vector3.one * scaleMultiplier;
        }
    }
}