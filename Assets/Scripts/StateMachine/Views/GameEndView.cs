using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Views
{
    public class GameEndView : BaseView
    {
        [SerializeField] private StarsContainerView _starsContainer; // TODO replace with stars view
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _retryButton;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _noStarsForYouText;
        
        public StarsContainerView StarsContainer => _starsContainer;
        public Button NextLevelButton => _nextLevelButton;
        public Button RetryButton => _retryButton;
        public TMP_Text ButtonText => _buttonText;
        public TMP_Text TitleText => _titleText;
        public TMP_Text NoStarsForYouText => _noStarsForYouText;
    }
}