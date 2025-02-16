using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Orders
{
    public class Order : IOrder
    {
        public event Action TimerExpired;
        public event Action OrderCompleted;
        public event Action OrderPartiallyCompleted;
        public List<WorkType> NeededTypes { get; }
        public Status Status { get; private set; }

        public Order(List<WorkType> neededTypes, float timeToComplete)
        {
            NeededTypes = neededTypes;
            Status = Status.Incomplete;
            StartTimer(timeToComplete).Forget();
        }

        public void ReceiveWork(WorkType workType)
        {
            if (Status == Status.Complete)
            {
                Debug.LogError("Order is already complete and cannot receive any more work");
                return;
            }

            if (NeededTypes.Contains(workType))
            {
                NeededTypes.Remove(workType);
                if (NeededTypes.Count != 0)
                {
                    OrderPartiallyCompleted?.Invoke();
                    return;
                }

                Status = Status.Complete;
                OrderCompleted?.Invoke();
            }
        }

        private async UniTaskVoid StartTimer(float timeToComplete)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeToComplete));
            TimerExpired?.Invoke();
        }
    }
}