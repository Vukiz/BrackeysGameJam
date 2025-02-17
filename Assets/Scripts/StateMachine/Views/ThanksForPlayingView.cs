using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Views
{
    public class ThanksForPlayingView : BaseView
    {
        [SerializeField] private Button _exitButton;

        public Button ExitButton => _exitButton;
    }
}