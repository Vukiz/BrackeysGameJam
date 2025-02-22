using Factories.Views;
using StateMachine.Views;
using UnityEngine;
using Workstation.Views;

namespace Tutorial.Views
{
    public class TutorialView : BaseView
    {
        [SerializeField] private WorkstationView _workstationView;
        [SerializeField] private FactoryView _factoryView;
         
        public WorkstationView WorkstationView => _workstationView;
    }
}