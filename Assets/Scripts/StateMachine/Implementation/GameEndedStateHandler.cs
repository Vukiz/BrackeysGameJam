using Cysharp.Threading.Tasks;
using Level.Data;
using Level.Infrastructure;
using StateMachine.Data;
using StateMachine.Infrastructure;
using StateMachine.Views;

namespace StateMachine.Implementation
{
    public class GameEndedStateHandler : GameStateHandlerBase
    {
        private readonly CanvasView _canvasView;
        private readonly IOrderProvider _orderProvider;
        private readonly IGameEndDataModel _gameEndDataModel;
        private readonly LevelsHolder _levelsHolder;
        private readonly IGameLevelDataModel _gameLevelDataModel;
        public override GameState State => GameState.GameEnded;

        public GameEndedStateHandler(
            CanvasView canvasView,
            IOrderProvider orderProvider,
            IGameEndDataModel gameEndDataModel,
            LevelsHolder levelsHolder,
            IGameLevelDataModel gameLevelDataModel
        )
        {
            _canvasView = canvasView;
            _orderProvider = orderProvider;
            _gameEndDataModel = gameEndDataModel;
            _levelsHolder = levelsHolder;
            _gameLevelDataModel = gameLevelDataModel;
        }

        public override async void OnStateEnter()
        {
            _canvasView.GameEndView.RetryButton.onClick.AddListener(OnRetryButtonClicked);
            var isGameWon = _gameEndDataModel.GameEndType == GameEndType.Win;
            if (isGameWon)
            {
                await SetupWinScreen();
            }
            else
            {
                await SetupLoseScreen();
            }
        }

        private void OnRetryButtonClicked()
        {
            _canvasView.ClickAudioSource.Play();
            RequestStateChange(GameState.GameActive);
        }

        private int GetReachedStars()
        {
            const int maxStars = 3;
            var failedOrders = _orderProvider.GetFailedOrdersCount();
            switch (failedOrders)
            {
                case 0:
                    return maxStars;
                case 1:
                    return 2;
                case 2:
                    return 1;
                default:
                    return 0;
            }
        }

        private async UniTask SetupLoseScreen()
        {
            _canvasView.GameEndView.RetryButton.gameObject.SetActive(true);
            _canvasView.GameEndView.NextLevelButton.gameObject.SetActive(false);
            _canvasView.GameEndView.ButtonText.text = "Retry!";
            _canvasView.GameEndView.TitleText.text = "Level Failed!";
            _canvasView.GameEndView.StarsContainer.gameObject.SetActive(false);
            _canvasView.GameEndView.NoStarsForYouText.gameObject.SetActive(true);
            

            await _canvasView.GameEndView.Show();
        }

        private async UniTask SetupWinScreen()
        {
            _canvasView.GameEndView.NoStarsForYouText.gameObject.SetActive(false);
            _canvasView.GameEndView.RetryButton.gameObject.SetActive(true);
            _canvasView.GameEndView.NextLevelButton.gameObject.SetActive(true);
            _canvasView.GameEndView.TitleText.text = "Level Complete!";
            _canvasView.GameEndView.ButtonText.text = IsLastLevel() ? "The End...?" : "Next!";

            _canvasView.GameEndView.StarsContainer.gameObject.SetActive(true);
            await _canvasView.GameEndView.Show();
            var stars = GetReachedStars();
            await _canvasView.GameEndView.StarsContainer.PlayStarsAnimation(stars);
            
            _canvasView.GameEndView.NextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        }

        private bool IsLastLevel()
        {
            return _gameLevelDataModel.CurrentLevelIndex > _levelsHolder.Levels.Count - 1;
        }

        private void OnNextLevelButtonClicked()
        {
            _canvasView.ClickAudioSource.Play();
            _gameLevelDataModel.CurrentLevelIndex++;
            _canvasView.GameEndView.NextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
            RequestStateChange(IsLastLevel() ? GameState.GameThanksForPlaying : GameState.GameActive);
        }

        public override void OnStateExit()
        {
            _canvasView.GameEndView.NextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
            _canvasView.GameEndView.Hide();
        }
    }
}