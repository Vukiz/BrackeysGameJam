using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Orders.Data;
using Orders.Infrastructure;
using UnityEngine;

namespace Orders.Implementation
{
    public class Order : IOrder
    {
        public event Action TimerExpired;
        public event Action OrderCompleted;
        public List<WorkType> NeededTypes { get; }
        public Status Status { get; private set; }
        public float Duration { get; private set; }

        public Order(List<WorkType> neededTypes, float timeToComplete)
        {
            NeededTypes = neededTypes;
            Status = Status.Incomplete;
            Duration = timeToComplete;
        }
        
        public void ReceiveWork(WorkType workType)
        {
            if (!NeededTypes.Contains(workType))
            {
                return;
            }

            NeededTypes.Remove(workType);
        }
        public void CheckWorkStatus()
        {
            if (Status == Status.Complete)
            {
                Debug.LogError("Order is already complete and cannot receive any more work");
                return;
            }

            if (NeededTypes.Count != 0)
            {
                return;
            }

            Status = Status.Complete;
            OrderCompleted?.Invoke();
        }

        public void StartTimer()
        {
            StartTimer(Duration).Forget();
        }

        private async UniTaskVoid StartTimer(float timeToComplete)
        {
            if (timeToComplete <= 0)
            {
                return;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(timeToComplete));
            TimerExpired?.Invoke();
        }
    }
}