using UnityEngine;

namespace StateMachine.Data
{
    [CreateAssetMenu(fileName = "GameLevelConfiguration", menuName = "Game/GameLevelConfiguration")]
    public class GameLevelConfiguration : ScriptableObject
    {
        private const string TutorialFinishedKey = "IsTutorialFinished";
        private const string CurrentLevelIndexKey = "CurrentLevelIndex";

        [SerializeField] private bool _isTutorialFinished;
        [SerializeField] private int _currentLevelIndex;

        public bool IsTutorialFinished
        {
            get => PlayerPrefs.GetInt(TutorialFinishedKey, _isTutorialFinished ? 1 : 0) == 1;
            set
            {
                PlayerPrefs.SetInt(TutorialFinishedKey, value ? 1 : 0);
                _isTutorialFinished = IsTutorialFinished;
            }
        }

        public int CurrentLevelIndex
        {
            get => PlayerPrefs.GetInt(CurrentLevelIndexKey, _currentLevelIndex);
            set
            {
                PlayerPrefs.SetInt(CurrentLevelIndexKey, value);
                _currentLevelIndex = CurrentLevelIndex;
            }
        }

    #if UNITY_EDITOR
        private void OnValidate()
        {
            // This ensures changes in editor are immediately reflected in PlayerPrefs
            IsTutorialFinished = _isTutorialFinished;
            CurrentLevelIndex = _currentLevelIndex;
        }
#endif
    }
}
