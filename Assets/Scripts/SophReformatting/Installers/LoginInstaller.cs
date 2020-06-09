using ClinicalTools.SimEncounters.MainMenu;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class LoginInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<StayLoggedIn>().To<StayLoggedIn>().AsTransient();
            Container.Bind<ILoginManager>().To<AutoLogin>().AsTransient();
            Container.Bind<IPasswordLoginManager>().To<PasswordLogin>().AsTransient();
            Container.Bind<UserParser>().To<UserParser>().AsTransient();
        }
    }
}