using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Orders.Data;
using UnityEngine;

namespace Factories.Views
{
    public class RobotView : MonoBehaviour
    {
        [SerializeField] private WorkType _workType;

        public WorkType WorkType => _workType;

        public event Action Destroyed;

        public UniTask MoveTo(Vector3 position, float duration, CancellationToken token)
        {
            LookAt(position);
            return transform.DOMove(position, duration)
                .SetEase(Ease.Linear)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        private void LookAt(Vector3 position)
        {
            position = new Vector3(position.x, transform.position.y, position.z);
            var directionToTarget = (position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(
                directionToTarget,
                Vector3.up
            );
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }
    }
}