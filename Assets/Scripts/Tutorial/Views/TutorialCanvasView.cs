using Cysharp.Threading.Tasks;
using StateMachine.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial.Views
{
    public class TutorialCanvasView : BaseView
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private TutorialDialogView _tutorialDialogView;
        [SerializeField] private TMP_Text _clickToContinue;
        
        public Button NextButton => _nextButton;
        public TutorialDialogView TutorialDialogView => _tutorialDialogView;
        
        public async UniTask WaitForNextButtonClick()
        {
            _clickToContinue.gameObject.SetActive(true);
            await _nextButton.OnClickAsync();
            _clickToContinue.gameObject.SetActive(false);
        }
    }
}