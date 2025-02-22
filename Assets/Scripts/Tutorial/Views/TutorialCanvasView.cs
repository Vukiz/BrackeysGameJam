using StateMachine.Views;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial.Views
{
    public class TutorialCanvasView : BaseView
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private TutorialDialogView _tutorialDialogView;
        
        public Button NextButton => _nextButton;
        public TutorialDialogView TutorialDialogView => _tutorialDialogView;
    }
}