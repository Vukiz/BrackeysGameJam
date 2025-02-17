using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Views
{
    public class GameActiveView : BaseView
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _muteButton;

        public Button ExitButton => _exitButton;
        public Button MuteButton => _muteButton;
    }
}