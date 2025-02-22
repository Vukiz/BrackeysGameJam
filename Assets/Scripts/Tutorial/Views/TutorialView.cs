using Factories.Views;
using Rails.Views;
using StateMachine.Views;
using UnityEngine;
using Workstation.Views;

namespace Tutorial.Views
{
    public class TutorialView : BaseView
    {
        [SerializeField] private WorkstationView _workstationView1;
        [SerializeField] private WorkstationView _workstationView2;
        [SerializeField] private FactorySlotView _factorySlot1;
        [SerializeField] private RailSwitchView _railSwitchView;
         
        public WorkstationView WorkstationView1 => _workstationView1;
        public WorkstationView WorkstationView2 => _workstationView2;
        public FactorySlotView FactorySlot1 => _factorySlot1;
        public RailSwitchView Switch => _railSwitchView;
    }
}