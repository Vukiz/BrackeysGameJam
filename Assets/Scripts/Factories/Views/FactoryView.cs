using Orders;
using Rails;
using Rails.Infrastructure;
using UnityEngine;

namespace Factories.View
{
    public class FactoryView : MonoBehaviour, IWaypointView
    {
        [SerializeField] private WorkType _workType;
        [SerializeField] private Transform _waypointTransform;
        [SerializeField] private Transform _robotSpawnPoint;

        public WorkType WorkType => _workType;
        public Transform WaypointTransform => _waypointTransform;
        public Transform RobotSpawnPoint => _robotSpawnPoint;
    }
}