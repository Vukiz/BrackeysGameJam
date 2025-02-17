using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Views
{
    public class GameEndView : BaseView
    {
        [SerializeField] private GameObject _starsContainer; // TODO replace with stars view
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private TMP_Text _buttonText;
        
        public GameObject StarsContainer => _starsContainer;
        public Button NextLevelButton => _nextLevelButton;
        public TMP_Text ButtonText => _buttonText;
    }
}