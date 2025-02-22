using StateMachine.Data;
using StateMachine.Infrastructure;
using StateMachine.Views;

namespace StateMachine.Implementation
{
    public class GameStartStateHandler : GameStateHandlerBase
    {
        private readonly CanvasView _canvasView;
        private readonly IGameLevelDataModel _gameLevelDataModel;
        public override GameState State => GameState.GameStart;

        public GameStartStateHandler(
            CanvasView canvasView,
            IGameLevelDataModel gameLevelDataModel
        )
        {
            _canvasView = canvasView;
            _gameLevelDataModel = gameLevelDataModel;
        }

        public override void OnStateEnter()
        {
            var canvasViewGameStartView = _canvasView.GameStartView;
            canvasViewGameStartView.gameObject.SetActive(true);
            canvasViewGameStartView.StartButton.onClick.AddListener(OnStartButtonClick);
            canvasViewGameStartView.ResetButton.gameObject.SetActive(_gameLevelDataModel.IsTutorialFinished);
            canvasViewGameStartView.ResetButton.onClick.AddListener(ResetSaveFile);
            canvasViewGameStartView.Show();
        }

        private void ResetSaveFile()
        {
            _gameLevelDataModel.ResetSaveFile();
            
            var canvasViewGameStartView = _canvasView.GameStartView;
            canvasViewGameStartView.ResetButton.gameObject.SetActive(false);
        }

        private async void OnStartButtonClick()
        {
            _canvasView.GameStartView.StartButton.onClick.RemoveListener(OnStartButtonClick);
            await _canvasView.LoaderView.Show();
            if (_gameLevelDataModel.IsTutorialFinished)
            {
                RequestStateChange(GameState.GameActive);
                return;
            }

            RequestStateChange(GameState.GameTutorial);
        }

        public override void OnStateExit()
        {
            var canvasViewGameStartView = _canvasView.GameStartView;
            canvasViewGameStartView.ResetButton.onClick.RemoveListener(ResetSaveFile);
            canvasViewGameStartView.Hide();
            canvasViewGameStartView.gameObject.SetActive(false);
        }
    }
}