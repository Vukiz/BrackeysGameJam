using Zenject;

namespace SushiBelt
{
    public class SushiBeltInstaller : Installer<SushiBeltInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<Implementation.SushiBelt>().AsTransient();
        }
    }
}