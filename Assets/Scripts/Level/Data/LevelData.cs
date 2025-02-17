using System;
using System.Collections.Generic;
using Level.Views;
using Orders;
using UnityEngine;

namespace Level.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
    public class LevelData : ScriptableObject
    {
        // If empty - orders will be randomized
        public List<OrderData> Orders;
        public LevelView LevelViewPrefab;
    }

    [Serializable]
    public class OrderData
    {
        public float TimeLimitSeconds;
        public List<WorkType> RequiredWorkTypes;
    }
}