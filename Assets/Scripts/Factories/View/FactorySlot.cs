using Level;
using Rails;
using Rails.Implementation;
using UnityEngine;

namespace Factories.View
{
    public class FactorySlot : MonoBehaviour, IInteractable
    {
        [SerializeField] private WaypointView _nextWaypointView;

        private bool _isEnabled;

        public IWaypointView NextWaypointView => _nextWaypointView;

        public event System.Action<FactorySlot> SlotSelected;

        public void Interact()
        {
            if (!_isEnabled)
            {
                return;
            }
            
            SlotSelected?.Invoke(this);
        }

        public void SetSlotButtonEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;
        }
    }
}