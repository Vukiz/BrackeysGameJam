using UnityEngine;

namespace StateMachine.Data
{
    [CreateAssetMenu(fileName = "GameLevelConfiguration", menuName = "Game/GameLevelConfiguration")]
    public class GameLevelConfiguration : ScriptableObject
    {
        private const string TutorialFinishedKey = "IsTutorialFinished";
        private const string CurrentLevelIndexKey = "CurrentLevelIndex";

        [SerializeField] private bool _defaultTutorialFinished;
        [SerializeField] private int _defaultCurrentLevelIndex;

        public bool IsTutorialFinished
        {
            get => PlayerPrefs.GetInt(TutorialFinishedKey, _defaultTutorialFinished ? 1 : 0) == 1;
            set => PlayerPrefs.SetInt(TutorialFinishedKey, value ? 1 : 0);
        }

        public int CurrentLevelIndex
        {
            get => PlayerPrefs.GetInt(CurrentLevelIndexKey, _defaultCurrentLevelIndex);
            set => PlayerPrefs.SetInt(CurrentLevelIndexKey, value);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // This ensures changes in editor are immediately reflected in PlayerPrefs
            IsTutorialFinished = _defaultTutorialFinished;
            CurrentLevelIndex = _defaultCurrentLevelIndex;
        }
#endif
    }
}
