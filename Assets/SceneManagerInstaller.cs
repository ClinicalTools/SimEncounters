using ClinicalTools.SimEncounters.MainMenu;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SceneManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MainMenuSceneStarter>().To<MainMenuSceneStarter>().AsTransient();
            Container.Bind<ReaderSceneStarter>().To<ReaderSceneStarter>().AsTransient();
            Container.Bind<IScenePathData>().To<MobileScenePathData>().AsTransient();
        }
    }
}