using System;
using Cysharp.Threading.Tasks;
using Factories.Data;
using Orders.Data;
using Orders.Infrastructure;
using Rails.Infrastructure;
using UnityEngine;

namespace Factories.Infrastructure
{
    public interface IRobot
    {
        event Action<IRobot, DestroyReason> RobotDestroyRequested;

        float SelfDestructionTimerDuration { set; }
        WorkType WorkType { get; }
        Vector3 Position { get; }
        Transform Transform { get; }
        bool IsTrackingRequired { get; set; }

        void SetNextWaypoint(IWaypoint waypoint, IIntermediateWaypoint intermediateWaypoint = null);
        UniTask CompleteOrder(IOrder order, Vector3 sushiBeltOrderPosition);
        void StartSelfDestructionTimer();
        void StopSelfDestructionTimer();

        void Destroy(DestroyReason destroyReason);
    }
}