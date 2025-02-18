using System.Collections.Generic;
using Factories.Views;
using Rails.Views;
using UnityEngine;
using Workstation.Views;

namespace Level.Views
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private List<WorkstationView> _workstationViews;
        [SerializeField] private List<RailSwitchView> _railSwitchViews;
        [SerializeField] private List<FactorySlotView> _factorySlots;
        [SerializeField] private Transform _factoriesParent;
        [SerializeField] private Transform _robotsParent;

        public List<WorkstationView> WorkstationViews => _workstationViews;
        public List<RailSwitchView> RailSwitchViews => _railSwitchViews;
        public List<FactorySlotView> FactorySlots => _factorySlots;
        public Transform FactoriesParent => _factoriesParent;
        public Transform RobotsParent => _robotsParent;
    }
}