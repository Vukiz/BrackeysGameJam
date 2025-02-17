using Level.Infrastructure;
using StateMachine.Data;
using StateMachine.Views;

namespace StateMachine.Implementation
{
    public class GameEndedStateHandler : GameStateHandlerBase
    {
        private readonly CanvasView _canvasView;
        private readonly IOrderProvider _orderProvider;
        public override GameState State => GameState.GameEnded;

        public GameEndedStateHandler(
            CanvasView canvasView,
            IOrderProvider orderProvider
        )
        {
            _canvasView = canvasView;
            _orderProvider = orderProvider;
        }

        public override async void OnStateEnter()
        {
            var isGameWon = _orderProvider.IsLevelWon();
            if (isGameWon)
            {
                SetupWinScreen();
            }
            else
            {
                SetupLoseScreen();
            }
            await _canvasView.GameEndView.Show();
            var stars = GetReachedStars();
            await _canvasView.GameEndView.StarsContainer.PlayStarsAnimation(stars);
            _canvasView.GameEndView.NextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
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

        private void SetupLoseScreen()
        {
            _canvasView.GameEndView.ButtonText.text = "Retry!";
            _canvasView.GameEndView.TitleText.text = "Level Failed!";
        }

        private void SetupWinScreen()
        {
            if (IsLastLevel())
            {
                _canvasView.GameEndView.ButtonText.text = "The End...?";
                _canvasView.GameEndView.TitleText.text = "Level Complete!";
            }
            else
            {
                _canvasView.GameEndView.ButtonText.text = "Next!";
                _canvasView.GameEndView.TitleText.text = "Level Complete!";
            }
        }
        
        private bool IsLastLevel()
        {
            return true; // TODO check if this is the last level
        }

        private void OnNextLevelButtonClicked()
        {
            _canvasView.GameEndView.NextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
            RequestStateChange(IsLastLevel() ? GameState.GameThanksForPlaying : GameState.GameActive);
        }

        public override void OnStateExit()
        {
            _canvasView.GameEndView.Hide();
        }
    }
}