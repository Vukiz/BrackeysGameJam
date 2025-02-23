using System.Collections.Generic;
using Factories.Implementation;
using Factories.Infrastructure;
using Level.Infrastructure;
using Workstation.Infrastructure;

namespace Level.Implementation
{
    public class LevelDataModel : ILevelDataModel
    {
        public List<IWorkstation> Workstations { get; } = new();
        public List<IFactory> Factories { get; } = new();
        public List<FactorySlot> FactorySlots { get; } = new();
    }
}