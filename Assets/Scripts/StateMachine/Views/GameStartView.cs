using UnityEngine;
using UnityEngine.UI;

namespace StateMachine.Views
{
    public class GameStartView : MonoBehaviour
    {
        [SerializeField] Button _startButton;

        public Button StartButton => _startButton;
    }
}