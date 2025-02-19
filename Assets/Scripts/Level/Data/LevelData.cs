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
        // They will go one by one in queue
        public List<RandomOrdersSettings> RandomOrdersSettings;
        public LevelView LevelViewPrefab;
    }
    
    [Serializable]
    public class RandomOrdersSettings
    {
        public int OrdersCount;
        public float TimeLimitSecondsMin;
        public float TimeLimitSecondsMax;
        public List<WorkType> WorkTypes;
    }

    [Serializable]
    public class OrderData
    {
        public float TimeLimitSeconds;
        public List<WorkType> RequiredWorkTypes;
    }
}