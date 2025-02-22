using StateMachine.Data;
using StateMachine.Infrastructure;

namespace StateMachine.Implementation
{
    public class GameLevelDataModel : IGameLevelDataModel
    {
        private readonly GameLevelConfiguration _configuration;

        public GameLevelDataModel(GameLevelConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsTutorialFinished
        {
            get => _configuration.IsTutorialFinished;
            set => _configuration.IsTutorialFinished = value;
        }

        public int CurrentLevelIndex
        {
            get => _configuration.CurrentLevelIndex;
            set => _configuration.CurrentLevelIndex = value;
        }
    }
}