namespace Workstation
{
    public class WorkstationInstaller : Zenject.Installer<WorkstationInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IWorkstation>().To<Workstation>().AsTransient();
        }
    }
}