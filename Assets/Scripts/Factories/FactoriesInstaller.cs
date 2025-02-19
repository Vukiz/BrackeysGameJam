using Factories.Implementation;
using Zenject;

namespace Factories
{
    public class FactoriesInstaller : Installer<FactoriesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<FactorySlot>().AsTransient();
            Container.BindInterfacesTo<FactoryAvailabilityTracker>()
                .AsSingle()
                .NonLazy();
            
            Container.Bind<FactoryProvider>()
                .FromSubContainerResolve()
                .ByMethod(subContainer =>
                {
                    subContainer.Bind<FactoryProvider>().AsSingle();
                    subContainer.BindInterfacesAndSelfTo<Factory>().AsTransient();
                }).AsSingle();

            Container.Bind<RobotProvider>()
                .FromSubContainerResolve()
                .ByMethod(subContainer =>
                {
                    subContainer.Bind<RobotProvider>().AsSingle();
                    subContainer.Bind<Robot>().AsTransient();
                }).AsSingle();
        }
    }
}