using System;
using Orders;
using UnityEngine;

namespace Factories.Data
{
    [Serializable]
    public class FactoryData
    {
        [SerializeField] private WorkType _workType;
        [SerializeField] private float _spawnCooldown;
        [SerializeField] private float _initialDelay;
        
        public WorkType WorkType => _workType;
        public float SpawnCooldown => _spawnCooldown;
        public float InitialDelay => _initialDelay;
    }
}