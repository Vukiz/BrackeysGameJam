using System.Collections.Generic;
using SushiBelt;
using UnityEngine;

namespace Workstation
{
    public class WorkstationView : MonoBehaviour
    {
        [SerializeField] private List<SlotView> _slotViews;
        [SerializeField] private SushiBeltView _sushiBeltView;
        [SerializeField] private Vector3 _workstationPosition;
        
        public List<SlotView> SlotViews => _slotViews;
        public SushiBeltView SushiBeltView => _sushiBeltView;
        public Vector3 Position => _workstationPosition;
    }
}