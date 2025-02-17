using System.Collections.Generic;
using UnityEngine;

namespace Level.Data
{
    [CreateAssetMenu(fileName = "LevelsHolder", menuName = "Levels/LevelsHolder")]
    public class LevelsHolder : ScriptableObject
    {
        [SerializeField] private List<LevelData> _levels;

        public List<LevelData> Levels => _levels;
    }
}