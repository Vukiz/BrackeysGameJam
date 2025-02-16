using Zenject;

namespace StateMachine
{
    public class StateMachineInstaller : Installer<StateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameMenuStateHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameActiveStateHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameEndedStateHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameThanksForPlayingStateHandler>().AsSingle();
        }
    }
}
