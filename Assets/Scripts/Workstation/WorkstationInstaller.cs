using Workstation.Infrastructure;

namespace Workstation
{
    public class WorkstationInstaller : Zenject.Installer<WorkstationInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IWorkstation>().To<Implementation.Workstation>().AsTransient();
        }
    }
}