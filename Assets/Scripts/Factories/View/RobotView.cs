using Orders;
using UnityEngine;

namespace Factories.View
{
    public class RobotView : MonoBehaviour
    {
        [SerializeField] private WorkType _workType;

        public WorkType WorkType => _workType;
    }
}