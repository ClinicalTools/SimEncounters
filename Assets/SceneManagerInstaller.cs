using ClinicalTools.SimEncounters.MainMenu;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SceneManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMenuSceneStarter>().To<MenuSceneStarter>().AsTransient();
            Container.Bind<IReaderSceneStarter>().To<ReaderSceneStarter>().AsTransient();
            Container.Bind<IScenePathData>().To<MobileScenePathData>().AsTransient();
        }
    }
}