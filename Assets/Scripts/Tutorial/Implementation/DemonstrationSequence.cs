using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Factories.Implementation;
using Level.Implementation;
using Orders.Data;
using Rails.Infrastructure;
using SushiBelt.Infrastructure;
using Tutorial.Infrastructure;
using Tutorial.Views;
using Workstation.Infrastructure;
using Workstation.Views;
using Zenject;
using IFactory = Factories.Infrastructure.IFactory;

namespace Tutorial.Implementation
{
    public class DemonstrationSequence : IDemonstrationSequence
    {
        private delegate UniTask Demonstration(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView);

        private readonly CameraProvider _cameraProvider;
        private readonly FactoryProvider _factoryProvider;
        private readonly DiContainer _container;
        private readonly List<Demonstration> _demonstrations = new();
        
        private IFactory _factory1;
        private IFactory _factory2;
        private IWorkstation _workstation1;
        private IWorkstation _workstation2;

        private IRailSwitch _railSwitch;

        public DemonstrationSequence(
            CameraProvider cameraProvider,
            FactoryProvider factoryProvider,
            DiContainer container
        )
        {
            _cameraProvider = cameraProvider;
            _factoryProvider = factoryProvider;
            _container = container;

            _demonstrations.AddRange(new List<Demonstration>
            {
                IntroduceWorkstation,
                IntroduceFactory,
                IntroduceRobotSpawn,
            });
        }

        private UniTask IntroduceRobotSpawn(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            
            return UniTask.CompletedTask;
        }

        public async UniTask Demonstrate(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            foreach (var demonstration in _demonstrations)
            {
                await demonstration(tutorialCanvasView, tutorialView);
            }
        }

        private async UniTask IntroduceWorkstation(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            _workstation1 = CreateWorkStation(tutorialView.WorkstationView1);
            _workstation2 = CreateWorkStation(tutorialView.WorkstationView2);
            tutorialCanvasView.TutorialDialogView.SetText(
                "Welcome to your first workstation! This is where the magic happens... " +
                "or catastrophic failures. Orders will appear here, and our enthusiastic robots will attempt to complete them. " +
                "\n\nEach workstation has 3 slots for robots.");
            tutorialCanvasView.TutorialDialogView.SetActive(true);

            await _cameraProvider.FocusOn(tutorialView.WorkstationView1.Position, 6f);

            await tutorialCanvasView.NextButton.OnClickAsync();
            tutorialCanvasView.TutorialDialogView.SetActive(false);
            await _cameraProvider.ResetToOriginalPosition();
        }

        private async UniTask IntroduceFactory(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            var railSwitchView = tutorialView.Switch;
            _railSwitch = _container.Resolve<IRailSwitch>();
            _railSwitch.SetView(railSwitchView);
            
            _railSwitch.AddNeighbour(_workstation1);
            _railSwitch.AddNeighbour(_workstation2);
            
            _factory1 = _factoryProvider.Create(
                WorkType.Paint, 
                tutorialView.FactorySlot1.transform.position,
                _railSwitch, 
                tutorialView.transform, 
                tutorialView.transform);
            
            tutorialCanvasView.TutorialDialogView.SetText("These lovely buildings are your robot factories! " +
                                                          "They'll automatically produce robots because someone lost the manual controls. " +
                                                          "\n\nKeep an eye on the production signal to see when your robots are ready to be deployed.");
            tutorialCanvasView.TutorialDialogView.SetActive(true);

            await _cameraProvider.FocusOn(tutorialView.FactorySlot1.transform.position, 6f);

            await tutorialCanvasView.NextButton.OnClickAsync();
            tutorialCanvasView.TutorialDialogView.SetActive(false);
            await _cameraProvider.ResetToOriginalPosition();
        }

        private IWorkstation CreateWorkStation(WorkstationView workstationView)
        {
            var sushiBelt = _container.Resolve<ISushiBelt>();
            sushiBelt.SetView(workstationView.SushiBeltView);
            var workstation = _container.Resolve<IWorkstation>();
            workstation.SetView(workstationView, sushiBelt);
            
            return workstation;
        }
    }
}