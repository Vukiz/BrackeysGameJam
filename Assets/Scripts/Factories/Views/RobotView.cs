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

        public UniTask MoveTo(Vector3 position, float duration, CancellationToken token)
        {
            LookAt(position);
            return transform.DOMove(position, duration)
                .SetEase(Ease.Linear)
                .WithCancellation(token);
        }

        private void LookAt(Vector3 position)
        {
            transform.rotation = Quaternion.LookRotation(position, Vector3.up);
        }
    }
}