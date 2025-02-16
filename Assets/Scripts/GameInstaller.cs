using Factories;
using Level;
using Orders;
using Rails;
using SushiBelt;
using Workstation;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        FactoriesInstaller.Install(Container);
        OrdersInstaller.Install(Container);
        RailsInstaller.Install(Container);
        WorkstationInstaller.Install(Container);
        SushiBeltInstaller.Install(Container);
        LevelInstaller.Install(Container);
    }
}        
