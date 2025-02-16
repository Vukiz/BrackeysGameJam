using System.Collections.Generic;
using System.Linq;
using Factories.Implementation;
using Factories.View;
using Orders;
using UnityEngine;

namespace Factories.Data
{
    [CreateAssetMenu(fileName = nameof(FactoriesConfiguration), menuName = "Configurations/FactoriesConfiguration")]
    public class FactoriesConfiguration : ScriptableObject
    {
        [SerializeField] private List<RobotView> _robotViews;
        [SerializeField] private List<RobotModel> _robotModels;
        [SerializeField] private List<FactoryView> _factoryViews;
        [SerializeField] private List<FactoryModel> _factoryModels;

        public RobotView GetRobotView(WorkType workType)
        {
            return _robotViews.FirstOrDefault(x => x.WorkType == workType);
        }

        public RobotModel GetRobotModel(WorkType workType)
        {
            return _robotModels.FirstOrDefault(x => x.WorkType == workType);
        }

        public FactoryView GetFactoryView(WorkType workType)
        {
            return _factoryViews.FirstOrDefault(x => x.WorkType == workType);
        }

        public FactoryModel GetFactoryModel(WorkType workType)
        {
            return _factoryModels.FirstOrDefault(x => x.WorkType == workType);
        }
    }
}