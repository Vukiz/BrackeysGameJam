using System.Collections.Generic;
using Rails.Implementation;
using SushiBelt.Views;
using UnityEngine;

namespace Workstation.Views
{
    public class WorkstationView : WaypointView
    {
        [SerializeField] private List<SlotView> _slotViews;
        [SerializeField] private SushiBeltView _sushiBeltView;
        [SerializeField] private Transform _workstationPosition;
        
        public List<SlotView> SlotViews => _slotViews;
        public SushiBeltView SushiBeltView => _sushiBeltView;
        public Vector3 Position => _workstationPosition.position;
    }
}