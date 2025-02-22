using UnityEngine;
using VFX.Implementation;

namespace Orders.Views
{
    public class ObjectView : MonoBehaviour
    {
        [SerializeField] private Outliner _outliner;

        public void SetActive(bool isActive, float duration)
        {
            gameObject.SetActive(isActive);
            if (isActive && duration > 0f)
            {
                _outliner.AnimateOutline(duration);
            }
        }

        public void DisableOutline()
        {
            _outliner.DisableOutline();
        }
    }
}