using UnityEngine;
using VFX.Implementation;

namespace Orders.Views
{
    public class ObjectView : MonoBehaviour
    {
        [SerializeField] private Outliner _outliner;
        [SerializeField] private GameObject _completedObject;
        [SerializeField] private GameObject _notCompletedObject;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
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

        public void SetObjectCompleted(bool isCompleted)
        {
            _completedObject.SetActive(isCompleted);
            _notCompletedObject.SetActive(!isCompleted);
        }
    }
}