using System.Collections.Generic;
using Rails;
using Rails.Implementation;
using Rails.Infrastructure;
using SushiBelt;
using SushiBelt.Views;
using UnityEngine;

namespace Workstation.Views
{
    public class WorkstationView : WaypointView
    {
        [SerializeField] private List<SlotView> _slotViews;
        [SerializeField] private SushiBeltView _sushiBeltView;
        [SerializeField] private Vector3 _workstationPosition;
        
        public List<SlotView> SlotViews => _slotViews;
        public SushiBeltView SushiBeltView => _sushiBeltView;
        public Vector3 Position => _workstationPosition;
    }
}