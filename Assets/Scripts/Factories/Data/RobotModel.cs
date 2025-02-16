using System;
using Orders;
using UnityEngine;

namespace Factories.Data
{
    [Serializable]
    public class RobotModel
    {
        [SerializeField] private WorkType _workType;

        public WorkType WorkType => _workType;
    }
}