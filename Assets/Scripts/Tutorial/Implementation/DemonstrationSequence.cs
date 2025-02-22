using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Level.Implementation;
using Tutorial.Infrastructure;
using Tutorial.Views;

namespace Tutorial.Implementation
{
    public class DemonstrationSequence : IDemonstrationSequence
    {
        private delegate UniTask Demonstration(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView);

        private readonly CameraProvider _cameraProvider;
        private readonly List<Demonstration> _demonstrations = new();

        public DemonstrationSequence(CameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;

            _demonstrations.AddRange(new List<Demonstration>
            {
                IntroduceWorkstation,
                IntroduceFactory,
            });
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
            tutorialCanvasView.TutorialDialogView.SetText(
                "Welcome to your first workstation! This is where the magic happens... " +
                "or catastrophic failures. Orders will appear here, and our enthusiastic robots will attempt to complete them. " +
                "\n\n<color=#FFD700>Each workstation has 3 slots for robots.</color>");
            tutorialCanvasView.TutorialDialogView.SetActive(true);

            // var hardcodedPosition = new UnityEngine.Vector3(-12, 10, 22);
            await _cameraProvider.FocusOn(tutorialView.WorkstationView.Position, 6f);

            await tutorialCanvasView.NextButton.OnClickAsync();
            tutorialCanvasView.TutorialDialogView.SetActive(false);
            await _cameraProvider.ResetToOriginalPosition();
        }

        private async UniTask IntroduceFactory(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            tutorialCanvasView.TutorialDialogView.SetText("These lovely buildings are your robot factories! " +
            "They'll automatically produce robots because someone lost the manual controls. " +
            "\n\nKeep an eye on the production signal to see when your robots are ready to be deployed."); 
            tutorialCanvasView.TutorialDialogView.SetActive(true);

            await _cameraProvider.FocusOn(tutorialView.FactoryView.transform.position, 6f);

            await tutorialCanvasView.NextButton.OnClickAsync();
            tutorialCanvasView.TutorialDialogView.SetActive(false);
            await _cameraProvider.ResetToOriginalPosition();
        }
    }
}