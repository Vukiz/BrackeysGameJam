using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Factories.Implementation;
using Factories.Infrastructure;
using Level.Implementation;
using Orders.Data;
using Orders.Implementation;
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
        private IWorkstation _workstation1;
        private IWorkstation _workstation2;

        private IRailSwitch _railSwitch;

        private IRobot _robot1;
        private IRobot _robot2;
        private IRobot _robot3;

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
                IntroduceOrders,
                DemonstrateWorkstationOverflow,
                IntroduceSwitch,
                Cleanup
            });
        }

        private UniTask Cleanup(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            _workstation1.Cleanup();
            _workstation2.Cleanup();
            _factory1.Cleanup();
            
            return UniTask.CompletedTask;
        }

        private async UniTask IntroduceSwitch(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            tutorialCanvasView.TutorialDialogView.SetText(
                "Switches help control traffic! Click them to redirect robots between workstations." +
                "\n\n<color=#FFD700>Try it now - click the switch!</color>");
            tutorialCanvasView.TutorialDialogView.SetActive(true);

            await _cameraProvider.FocusOn(_railSwitch.Position, 6f);
            tutorialCanvasView.NextButton.gameObject.SetActive(false);
            _railSwitch.SetInteractable(true);
            _railSwitch.SetOutlineEnabled(true);
            var rotateCompletionSource = new UniTaskCompletionSource();
            _railSwitch.Rotated += () =>
            {
                rotateCompletionSource.TrySetResult();
            };

            await rotateCompletionSource.Task;
            tutorialCanvasView.NextButton.gameObject.SetActive(true);
            _railSwitch.SetOutlineEnabled(false);
            await _cameraProvider.ResetToOriginalPosition();
            tutorialCanvasView.TutorialDialogView.SetText(
                "Great! Now you can control robot flow. Keep an eye on workstation capacity!");
            _robot1 = await _factory1.SpawnRobot();
            _robot1.SelfDestructionTimerDuration = 0f;
            await tutorialCanvasView.WaitForNextButtonClick();
            tutorialCanvasView.TutorialDialogView.SetActive(false);
        }

        private async UniTask DemonstrateWorkstationOverflow(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            tutorialCanvasView.TutorialDialogView.SetText(
                "Watch out for workstation overload! Full slots will explode with new robots." +
                "\n\n<color=#FFD700>Warning: Game over if all slots are destroyed!</color>");
            tutorialCanvasView.TutorialDialogView.SetActive(true);

            var robot = await _factory1.SpawnRobot();
            robot.SelfDestructionTimerDuration = 0f;
            var robot2 = await _factory1.SpawnRobot();
            robot2.SelfDestructionTimerDuration = 0f;
            UniTaskCompletionSource robotExploded = new();
            _workstation1.SlotRemoved += () => robotExploded.TrySetResult();

            await _cameraProvider.FocusOn(robot.Position, 6f);
            _cameraProvider.StartTracking(robot.Transform).Forget();
            await robotExploded.Task;
            
            await tutorialCanvasView.WaitForNextButtonClick();
            tutorialCanvasView.TutorialDialogView.SetActive(false);
            await _cameraProvider.ResetToOriginalPosition();
        }

        private async UniTask IntroduceOrders(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            tutorialCanvasView.TutorialDialogView.SetText(
                "Orders appear at workstations showing required robots. Complete them before time runs out!" +
                "\n\n<color=#FFD700>Missed orders will move along.</color>");
            tutorialCanvasView.TutorialDialogView.SetActive(true);

            await _cameraProvider.FocusOn(_workstation1.Position, 6f);

            var order = new Order(
                new List<WorkType> { _factory1.WorkType }, 
                0f // No time limit
            );

            _workstation1.AddOrderToSushiBelt(order);
            await UniTask.Delay(TimeSpan.FromSeconds(2f));

            await tutorialCanvasView.WaitForNextButtonClick();
            tutorialCanvasView.TutorialDialogView.SetActive(false);
            await _cameraProvider.ResetToOriginalPosition();
        }

        private async UniTask IntroduceRobotSpawn(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            tutorialCanvasView.TutorialDialogView.SetText(
                "Meet your robots! They'll complete matching orders at workstations." +
                "\n\n<color=#FFD700>Note: Robots have limited lifespan!</color>");
            tutorialCanvasView.TutorialDialogView.SetActive(true);

            _robot1 = await _factory1.SpawnRobot();
            _robot1.SelfDestructionTimerDuration = 0f;

             _cameraProvider.StartTracking(_robot1.Transform).Forget();
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            _robot2 = await _factory1.SpawnRobot();
            _robot2.SelfDestructionTimerDuration = 0f;
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            _robot3 = await _factory1.SpawnRobot();
            _robot3.SelfDestructionTimerDuration = 0f; // don't let the robots self explode
            await _cameraProvider.ResetToOriginalPosition();

            await tutorialCanvasView.WaitForNextButtonClick();
            tutorialCanvasView.TutorialDialogView.SetActive(false);
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
                "Welcome to your workstation! Orders appear here for robots to complete." +
                "\n\nEach workstation has 3 slots.");
            tutorialCanvasView.TutorialDialogView.SetActive(true);

            await _cameraProvider.FocusOn(tutorialView.WorkstationView1.Position, 6f);

            await tutorialCanvasView.WaitForNextButtonClick();
            tutorialCanvasView.TutorialDialogView.SetActive(false);
            await _cameraProvider.ResetToOriginalPosition();
        }

        private async UniTask IntroduceFactory(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            tutorialCanvasView.TutorialDialogView.SetText(
                "Time to build! Place factories on glowing spots. New types unlock with orders." +
                "\n\n<color=#FFD700>Click the highlight to place your factory!</color>");
            tutorialCanvasView.TutorialDialogView.SetActive(true);
            var slotSelectedTaskCompletionSource = new UniTaskCompletionSource();
            tutorialView.FactorySlot1.SlotSelected += _ => slotSelectedTaskCompletionSource.TrySetResult();
            tutorialView.FactorySlot1.SetInteractable(true);

            await _cameraProvider.FocusOn(tutorialView.FactorySlot1.transform.position, 6f);
            await slotSelectedTaskCompletionSource.Task;
            tutorialView.FactorySlot1.SetInteractable(false);
            
            var railSwitchView = tutorialView.Switch;
            _railSwitch = _container.Resolve<IRailSwitch>();
            _railSwitch.SetView(railSwitchView);
            _railSwitch.SetInteractable(false);

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

            await tutorialCanvasView.WaitForNextButtonClick();
            tutorialCanvasView.TutorialDialogView.SetActive(false);
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