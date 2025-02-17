using System;
using Orders;
using UnityEngine;

namespace Factories.Data
{
    [Serializable]
    public class RobotData
    {
        [SerializeField] private WorkType _workType;
        [SerializeField] private float _speed;

        public WorkType WorkType => _workType;
        public float Speed => _speed;
    }
}