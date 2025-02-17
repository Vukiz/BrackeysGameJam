using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Views
{
    public class GameStartView : BaseView
    {
        [SerializeField] Button _startButton;

        public Button StartButton => _startButton;
    }
}