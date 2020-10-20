using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSelectorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ReaderEncounterManager>().AsSingle();

            Container.BindInterfacesTo<LoadingReaderSceneInfoSelector>().AsSingle();
            Container.BindInterfacesTo<ReaderSceneInfoSelector>().AsSingle();

            Container.Bind<ILinearEncounterNavigator>().To<LinearEncounterNavigator>().AsSingle();
            Container.Bind<ICompletionHandler>().To<CompletionHandler>().AsSingle();
        }
    }
}