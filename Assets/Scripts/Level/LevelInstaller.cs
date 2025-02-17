using Level.Implementation;
using Level.Infrastructure;
using Zenject;

namespace Level
{
    public class LevelInstaller : Installer<LevelInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<WaypointProvider>().AsSingle();
            Container.Bind<CameraProvider>().AsSingle();
            Container.BindInterfacesTo<InputTracker>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesTo<LevelConfigurator>().AsSingle();
        }
    }
}