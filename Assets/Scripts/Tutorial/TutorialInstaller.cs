using Tutorial.Implementation;
using Zenject;

namespace Tutorial
{
    public class TutorialInstaller : Installer<TutorialInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DemonstrationSequence>().AsSingle();
            
        }
    }
}