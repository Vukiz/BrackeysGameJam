using System.Collections.Generic;
using Factories.Implementation;
using Factories.Infrastructure;
using Workstation.Infrastructure;

namespace Level.Infrastructure
{
    public interface ILevelDataModel
    {
        public List<IWorkstation> Workstations { get; }
        public List<IFactory> Factories { get; }
        List<FactorySlot> FactorySlots { get; }
    }
}