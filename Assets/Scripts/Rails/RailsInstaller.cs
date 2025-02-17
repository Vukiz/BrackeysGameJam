using Rails.Implementation;

namespace Rails
{
    public class RailsInstaller : Zenject.Installer<RailsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RailSwitch>().AsTransient();
        }
    }
}