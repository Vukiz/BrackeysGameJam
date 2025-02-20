using System;
using Orders;
using Orders.Data;
using UnityEngine;

namespace Factories.Data
{
    [Serializable]
    public class RobotData
    {
        [SerializeField] private WorkType _workType;
        [SerializeField] private float _speed;
        [SerializeField] private float _selfDestructionTimerDuration;

        public WorkType WorkType => _workType;
        public float Speed => _speed;
        public float SelfDestructionTimerDuration => _selfDestructionTimerDuration;
    }
}