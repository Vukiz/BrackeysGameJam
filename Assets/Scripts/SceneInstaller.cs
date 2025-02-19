using Factories;
using Level;
using Orders;
using Rails;
using StateMachine;
using StateMachine.Views;
using SushiBelt;
using UnityEngine;
using VFX;
using Workstation;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CanvasView _canvas;
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
        Container.Bind<CanvasView>().FromInstance(_canvas);

        FactoriesInstaller.Install(Container);
        OrdersInstaller.Install(Container);
        RailsInstaller.Install(Container);
        WorkstationInstaller.Install(Container);
        SushiBeltInstaller.Install(Container);
        LevelInstaller.Install(Container);
        StateMachineInstaller.Install(Container);
        VFXInstaller.Install(Container);
    }
}        
