using Zenject;

namespace Level
{
    public class LevelInstaller : Installer<LevelInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WaypointProvider>().AsSingle();
            Container.Bind<CameraProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputTracker>()
                .AsSingle()
                .NonLazy();

            Container.Bind<LevelConfigurator>().AsSingle();
        }
    }
}