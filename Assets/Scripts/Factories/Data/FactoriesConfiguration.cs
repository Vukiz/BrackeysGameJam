using System.Collections.Generic;
using System.Linq;
using Factories.Views;
using Orders;
using Orders.Data;
using UnityEngine;

namespace Factories.Data
{
    [CreateAssetMenu(fileName = nameof(FactoriesConfiguration), menuName = "Configurations/FactoriesConfiguration")]
    public class FactoriesConfiguration : ScriptableObject
    {
        [SerializeField] private List<RobotView> _robotViews;
        [SerializeField] private List<RobotData> _robotData;
        [SerializeField] private List<FactoryView> _factoryViews;
        [SerializeField] private List<FactoryData> _factoryData;

        public RobotView GetRobotView(WorkType workType)
        {
            return _robotViews.FirstOrDefault(x => x.WorkType == workType);
        }

        public RobotData GetRobotData(WorkType workType)
        {
            return _robotData.FirstOrDefault(x => x.WorkType == workType);
        }

        public FactoryView GetFactoryView(WorkType workType)
        {
            return _factoryViews.FirstOrDefault(x => x.WorkType == workType);
        }

        public FactoryData GetFactoryData(WorkType workType)
        {
            return _factoryData.FirstOrDefault(x => x.WorkType == workType);
        }
    }
}