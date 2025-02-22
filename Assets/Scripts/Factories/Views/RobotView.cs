using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Orders.Data;
using UnityEngine;
using VFX.Implementation;

namespace Factories.Views
{
    public class RobotView : MonoBehaviour
    {
        [SerializeField] private WorkType _workType;
        [SerializeField] private Outliner _outliner;
        
        private Vector3 _initialScale;

        public WorkType WorkType => _workType;

        public event Action Destroyed;

        private void Awake()
        {
            _initialScale = transform.localScale;
        }

        public UniTask MoveTo(Vector3 position, float duration, CancellationToken token)
        {
            LookAt(position);
            return transform.DOMove(position, duration)
                .SetEase(Ease.Linear)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        public void LookAt(Vector3 position)
        {
            position = new Vector3(position.x, transform.position.y, position.z);
            var directionToTarget = (position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(
                directionToTarget,
                Vector3.up
            );
        }

        public void AnimateOverheat(float duration)
        {
            _outliner.AnimateOutline(duration);
        }

        public void DisableOutline()
        {
            _outliner.DisableOutline();
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }
    }
}