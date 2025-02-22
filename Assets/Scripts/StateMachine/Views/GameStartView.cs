using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Views
{
    public class GameStartView : BaseView
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _resetButton;

        public Button StartButton => _startButton;
        public Button ResetButton => _resetButton;
    }
}