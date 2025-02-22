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

            _demonstrations.Add(ShowWorkstationTutorial);
        }


        public async UniTask Demonstrate(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            foreach (var demonstration in _demonstrations)
            {
                await demonstration(tutorialCanvasView, tutorialView);
            }
        }

        private async UniTask ShowWorkstationTutorial(TutorialCanvasView tutorialCanvasView, TutorialView tutorialView)
        {
            tutorialCanvasView.TutorialDialogView.SetActive(true);
            tutorialCanvasView.TutorialDialogView.SetText("Welcome to your first workstation! This is where the magic happens... " +
                "or catastrophic failures. Orders will appear here, and our enthusiastic robots will attempt to complete them. " +
                "\n\n<color=#FFD700>Each workstation has 3 slots for robots.</color>");

            await _cameraProvider.FocusOn(tutorialView.WorkstationView.transform.position);

            await tutorialCanvasView.NextButton.OnClickAsync();

            await _cameraProvider.ResetToOriginalPosition();
        }

    }
}