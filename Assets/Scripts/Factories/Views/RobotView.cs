using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Orders;
using UnityEngine;

namespace Factories.View
{
    public class RobotView : MonoBehaviour
    {
        [SerializeField] private WorkType _workType;

        public WorkType WorkType => _workType;

        public UniTask MoveTo(Vector3 position, float duration, CancellationToken token)
        {
            LookAt(position);
            return transform.DOMove(position, duration).WithCancellation(token);
        }

        private void LookAt(Vector3 position)
        {
            var rotation = Vector3.RotateTowards(transform.forward, position, 0.1f, 0f);
            transform.rotation = Quaternion.LookRotation(rotation);
        }
    }
}