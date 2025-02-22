using Cysharp.Threading.Tasks;
using Level.Data;
using StateMachine.Data;
using StateMachine.Infrastructure;
using StateMachine.Views;
using Tutorial.Views;
using UnityEngine;
using Zenject;

namespace StateMachine.Implementation
{
    public class GameTutorialStateHandler : GameStateHandlerBase
    {
        private readonly CanvasView _canvasView;
        private readonly IGameLevelDataModel _gameLevelDataModel;
        private readonly LevelsHolder _levelsHolder;
        private readonly IInstantiator _instantiator;
        public override GameState State => GameState.GameTutorial;

        private TutorialView _tutorialView;

        public GameTutorialStateHandler(
            CanvasView canvasView,
            IGameLevelDataModel gameLevelDataModel,
            LevelsHolder levelsHolder,
            IInstantiator instantiator
        )
        {
            _canvasView = canvasView;
            _gameLevelDataModel = gameLevelDataModel;
            _levelsHolder = levelsHolder;
            _instantiator = instantiator;
        }

        public override async void OnStateEnter()
        {
            _canvasView.TutorialCanvasView.NextButton.onClick.AddListener(OnNextButtonClicked);
            _tutorialView = _instantiator.InstantiatePrefabForComponent<TutorialView>(_levelsHolder.TutorialViewPrefab);

            await _canvasView.LoaderView.Hide();
            
        }

        // full screen button that will progress the tutorial
        private void OnNextButtonClicked()
        {
            
        }

        private async UniTask TutorialCompleted()
        {
            await _canvasView.LoaderView.Show();
        }

        public override void OnStateExit()
        {
            Object.Destroy(_tutorialView.gameObject);
            _gameLevelDataModel.IsTutorialFinished = true;
        }
    }
}