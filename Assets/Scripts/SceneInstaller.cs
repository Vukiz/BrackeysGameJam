using Factories;
using Level;
using Orders;
using Rails;
using StateMachine;
using SushiBelt;
using UnityEngine;
using Workstation;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera;
    public override void InstallBindings()
    {
        FactoriesInstaller.Install(Container);
        OrdersInstaller.Install(Container);
        RailsInstaller.Install(Container);
        WorkstationInstaller.Install(Container);
        SushiBeltInstaller.Install(Container);
        LevelInstaller.Install(Container);
        StateMachineInstaller.Install(Container);
        
        var cameraProvider = Container.Resolve<CameraProvider>(); // TODO: Maybe move camera initialization somewhere else
        cameraProvider.SetMainCamera(_mainCamera);
    }
}        
