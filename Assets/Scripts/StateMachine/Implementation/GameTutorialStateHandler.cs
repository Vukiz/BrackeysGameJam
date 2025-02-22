using Level.Data;
using StateMachine.Data;
using StateMachine.Infrastructure;
using StateMachine.Views;
using Tutorial.Infrastructure;
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
        private readonly IDemonstrationSequence _demonstrationSequence;
        public override GameState State => GameState.GameTutorial;

        private TutorialView _tutorialView;

        public GameTutorialStateHandler(
            CanvasView canvasView,
            IGameLevelDataModel gameLevelDataModel,
            LevelsHolder levelsHolder,
            IInstantiator instantiator,
            IDemonstrationSequence demonstrationSequence
        )
        {
            _canvasView = canvasView;
            _gameLevelDataModel = gameLevelDataModel;
            _levelsHolder = levelsHolder;
            _instantiator = instantiator;
            _demonstrationSequence = demonstrationSequence;
        }

        public override async void OnStateEnter()
        {
            _tutorialView = _instantiator.InstantiatePrefabForComponent<TutorialView>(_levelsHolder.TutorialViewPrefab);
            await _canvasView.TutorialCanvasView.Show();
            await _canvasView.LoaderView.Hide();
            await _demonstrationSequence.Demonstrate(_canvasView.TutorialCanvasView, _tutorialView);
            await _canvasView.LoaderView.Show();
            RequestStateChange(GameState.GameActive);
        }

        public override void OnStateExit()
        {
            _canvasView.TutorialCanvasView.Hide();
            Object.Destroy(_tutorialView.gameObject);
            _gameLevelDataModel.IsTutorialFinished = true;
        }
    }
}