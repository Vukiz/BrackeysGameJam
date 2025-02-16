using System.Collections.Generic;

namespace Orders
{
    public interface IOrder
    {
        List<WorkType> NeededTypes { get; }
        Status Status { get; }
        
        void ReceiveWork(WorkType workType);

        event System.Action TimerExpired;
        event System.Action OrderCompleted;
        event System.Action OrderPartiallyCompleted;
    }
}