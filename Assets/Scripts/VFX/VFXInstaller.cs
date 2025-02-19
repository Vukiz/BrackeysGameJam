using Zenject;

namespace VFX
{
    public class VFXInstaller : Installer<VFXInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<Implementation.VFXManager>().AsSingle();
        }
    }
}