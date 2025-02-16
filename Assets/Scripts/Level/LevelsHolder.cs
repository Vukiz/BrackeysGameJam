using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelsHolder", menuName = "Levels/LevelsHolder")]
    public class LevelsHolder : ScriptableObjectInstaller
    {
        [SerializeField] private List<LevelData> _levels;

        public List<LevelData> Levels => _levels;
    }
}