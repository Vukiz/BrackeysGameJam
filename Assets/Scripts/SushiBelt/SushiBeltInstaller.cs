using SushiBelt.Infrastructure;
using Zenject;

namespace SushiBelt
{
    public class SushiBeltInstaller : Installer<SushiBeltInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ISushiBelt>().To<Implementation.SushiBelt>().AsTransient();
        }
    }
}