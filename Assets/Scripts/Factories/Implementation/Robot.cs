using System;
using Factories.Data;
using Factories.View;
using Orders;
using Rails;

namespace Factories.Implementation
{
    public class Robot : IRobot
    {
        private RobotView _view;
        private RobotModel _model;
        
        public WorkType WorkType => _model.WorkType;

        public event Action CollisionDetected;


        public void Initialize(RobotView view, RobotModel model)
        {
            _view = view;
            _model = model;
        }
        
        public void SetNextWaypoint(IWaypoint waypoint)
        {
            // TODO: Add movement and intermediate point
        }

        public void CompleteOrder(IOrder order)
        {
            
        }
    }
}