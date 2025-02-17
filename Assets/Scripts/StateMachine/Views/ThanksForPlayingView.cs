using UnityEngine;

namespace StateMachine.Views
{
    public class ThanksForPlayingView : MonoBehaviour
    {
        [SerializeField] private GameObject _exitButton;

        public GameObject ExitButton => _exitButton;
    }
}