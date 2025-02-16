using System.Collections.Generic;
using Orders;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
    public class LevelData : ScriptableObject
    {
        // If empty - orders will be randomized
        public List<OrderData> Orders;
        public LevelView LevelViewPrefab;
    }

    public class OrderData
    {
        public float TimeLimitSeconds;
        public WorkType[] RequiredWorkTypes;
        
    }
}