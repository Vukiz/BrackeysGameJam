using Level.Infrastructure;
using Rails.Implementation;
using Rails.Infrastructure;
using UnityEngine;

namespace Factories.View
{
    public class FactorySlot : MonoBehaviour, IInteractable
    {
        [SerializeField] private WaypointView _nextWaypointView;

        private bool _isInteractable;

        public IWaypointView NextWaypointView => _nextWaypointView;

        public event System.Action<FactorySlot> SlotSelected;

        public void Interact()
        {
            if (!_isInteractable)
            {
                return;
            }
            
            SlotSelected?.Invoke(this);
        }

        public void SetInteractable(bool isInteractable)
        {
            _isInteractable = isInteractable;
        }
    }
}