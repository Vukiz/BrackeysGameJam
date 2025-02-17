using System;
using System.Collections.Generic;
using Level;
using Rails.Implementation;
using UnityEngine;

namespace Rails.View
{
    public class RailSwitchView : WaypointView, IInteractable
    {
        [SerializeField] private List<WaypointView> _neighbourWaypoints;

        public List<WaypointView> NeighbourWaypoints => _neighbourWaypoints;

        public event Action InteractionRequired;
        
        public void Interact()
        {
            InteractionRequired?.Invoke();
        }
    }
}