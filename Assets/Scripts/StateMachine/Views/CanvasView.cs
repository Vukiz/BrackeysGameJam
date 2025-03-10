using Tutorial.Views;
using UnityEngine;

namespace StateMachine.Views
{
    public class CanvasView : MonoBehaviour
    {
        [SerializeField] private GameActiveView _gameActiveView;
        [SerializeField] private GameEndView _gameEndView;
        [SerializeField] private GameStartView _gameStartView;
        [SerializeField] private ThanksForPlayingView _thanksForPlayingView;
        [SerializeField] private LoaderView _loaderView;
        [SerializeField] private TutorialCanvasView _tutorialCanvasView;
        [SerializeField] private AudioSource _clickAudioSource;

        public GameActiveView GameActiveView => _gameActiveView;
        public GameEndView GameEndView => _gameEndView;
        public GameStartView GameStartView => _gameStartView;
        public ThanksForPlayingView ThanksForPlayingView => _thanksForPlayingView;
        public LoaderView LoaderView => _loaderView;
        public TutorialCanvasView TutorialCanvasView => _tutorialCanvasView;
        public AudioSource ClickAudioSource => _clickAudioSource;
    }
}