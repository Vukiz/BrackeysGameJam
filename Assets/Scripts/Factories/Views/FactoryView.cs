using System;
using Orders.Data;
using Rails.Infrastructure;
using UnityEngine;
using VFX.Views;

namespace Factories.Views
{
    public class FactoryView : MonoBehaviour, IWaypointView
    {
        [SerializeField] private SignalGlow _signalGlow;
        [SerializeField] private WorkType _workType;
        [SerializeField] private Transform _waypointTransform;
        [SerializeField] private Transform _robotSpawnPoint;

        public WorkType WorkType => _workType;
        public Transform RobotSpawnPoint => _robotSpawnPoint;
        public SignalGlow SignalGlow => _signalGlow;

        public event Action Destroyed;

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }
    }
}