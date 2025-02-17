using System.Collections.Generic;
using Factories.View;
using Rails.View;
using SushiBelt;
using UnityEngine;
using Workstation;

namespace Level
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private List<SushiBeltView> _sushiBeltViews;
        [SerializeField] private List<WorkstationView> _workstationViews;
        [SerializeField] private List<RailSwitchView> _railSwitchViews;
        [SerializeField] private List<FactorySlot> _factorySlots;

        public List<SushiBeltView> SushiBeltViews => _sushiBeltViews;
        public List<WorkstationView> WorkstationViews => _workstationViews;
        public List<RailSwitchView> RailSwitchViews => _railSwitchViews;
        public List<FactorySlot> FactorySlots => _factorySlots;
    }
}