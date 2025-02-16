using System.Collections.Generic;

namespace Orders
{
    public interface IOrder
    {
        List<WorkType> NeededTypes { get; }
        Status Status { get; }

        event System.Action TimerExpired;
    }
}