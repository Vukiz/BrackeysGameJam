using System.Collections.Generic;
using Orders.Data;

namespace Orders.Infrastructure
{
    public interface IOrder
    {
        List<WorkType> NeededTypes { get; }
        Status Status { get; }
        float Duration { get; }
        
        void ReceiveWork(WorkType workType);

        event System.Action TimerExpired;
        event System.Action OrderCompleted;
        void CheckWorkStatus();
        void StartTimer();
    }
}