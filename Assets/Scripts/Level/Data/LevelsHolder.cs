using System.Collections.Generic;
using Tutorial.Views;
using UnityEngine;

namespace Level.Data
{
    [CreateAssetMenu(fileName = "LevelsHolder", menuName = "Levels/LevelsHolder")]
    public class LevelsHolder : ScriptableObject
    {
        [SerializeField] private TutorialView _tutorialViewPrefab;
        [SerializeField] private List<LevelData> _levels;

        public List<LevelData> Levels => _levels;
        public TutorialView TutorialViewPrefab => _tutorialViewPrefab;
    }
}