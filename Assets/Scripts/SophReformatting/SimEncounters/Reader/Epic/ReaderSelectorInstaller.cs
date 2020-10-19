using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSelectorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISelector<LoadingReaderSceneInfo>>().To<LoadingReaderSceneInfoSelector>().AsSingle();
            Container.Bind<ISelector<ReaderSceneInfo>>().To<ReaderSceneInfoSelector>().AsSingle();

            Container.Bind<ISelector<UserEncounterSelectedEventArgs>>().To<UserEncounterSelector>().AsSingle();
            Container.Bind<ISelector<UserSectionSelectedEventArgs>>().To<UserSectionSelector>().AsSingle();
            Container.Bind<ISelector<UserTabSelectedEventArgs>>().To<UserTabSelector>().AsSingle();

            Container.Bind<ISelector<Encounter>>().To<EncounterSelector>().AsSingle();

            Container.Bind<ISelector<EncounterMetadata>>().To<Selector<EncounterMetadata>>().AsSingle();
            Container.Bind<ISelector<SectionSelectedEventArgs>>().To<Selector<SectionSelectedEventArgs>>().AsSingle();
            Container.Bind<ISelector<TabSelectedEventArgs>>().To<Selector<TabSelectedEventArgs>>().AsSingle();

            Container.Bind<ILinearEncounterNavigator>().To<LinearEncounterNavigator>().AsSingle();
            Container.Bind<ICompletionHandler>().To<CompletionHandler>().AsSingle();
        }
    }
}