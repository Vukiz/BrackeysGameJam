using UnityEngine;

namespace Workstation.Views
{
    public class SlotView : MonoBehaviour
    {
        [SerializeField] private GameObject _destroyedSlot;
        [SerializeField] private GameObject _normalSlot;

        public void SetDestroyed(bool isDestroyed)
        {
            _destroyedSlot.SetActive(isDestroyed);
            _normalSlot.SetActive(!isDestroyed);
        }
    }
}