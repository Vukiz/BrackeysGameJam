using Zenject;

namespace SushiBelt
{
    public class SushiBeltInstaller : Installer<SushiBeltInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ISushiBelt>().To<SushiBelt>().AsTransient();
        }
    }
}