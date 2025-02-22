using StateMachine.Infrastructure;
using UnityEngine;

namespace StateMachine.Implementation
{
    public class GameLevelDataModel : IGameLevelDataModel
    {
        public bool IsTutorialFinished
        {
            get => PlayerPrefs.GetInt("IsTutorialFinished", 0) == 1;
            set => PlayerPrefs.SetInt("IsTutorialFinished", value ? 1 : 0);
        }

        public int CurrentLevelIndex
        {
            get => PlayerPrefs.GetInt("CurrentLevelIndex", 0);
            set => PlayerPrefs.SetInt("CurrentLevelIndex", value);
        }
    }
}