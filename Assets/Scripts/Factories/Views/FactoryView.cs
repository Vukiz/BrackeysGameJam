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
        [SerializeField] private Transform _robotLaunchPoint;
        [SerializeField] private Animator _doorsAnimator;
        [SerializeField] private AnimationCurve _launchCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public WorkType WorkType => _workType;
        public Transform RobotSpawnPoint => _robotSpawnPoint;
        public SignalGlow SignalGlow => _signalGlow;
        public Animator DoorsAnimator => _doorsAnimator;
        public AnimationCurve LaunchCurve => _launchCurve;
        public Transform RobotLaunchPoint => _robotLaunchPoint;

        public event Action Destroyed;

        public void LookAt(Vector3 position)
        {
            position = new Vector3(position.x, transform.position.y, position.z);
            var direction = (position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }
    }
}