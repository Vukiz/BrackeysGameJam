using StateMachine.Implementation;
using Zenject;

namespace StateMachine
{
    public class StateMachineInstaller : Installer<StateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
            Container.BindInterfacesTo<GameStartStateHandler>().AsSingle();
            Container.BindInterfacesTo<GameActiveStateHandler>().AsSingle();
            Container.BindInterfacesTo<GameEndedStateHandler>().AsSingle();
            Container.BindInterfacesTo<GameThanksForPlayingStateHandler>().AsSingle();
            Container.BindInterfacesTo<GameTutorialStateHandler>().AsSingle();
            Container.BindInterfacesTo<GameLevelDataModel>().AsSingle();
            Container.BindInterfacesTo<GameEndDataModel>().AsSingle();
        }
    }
}
