using ClinicalTools.UI;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MenuInstaller : MonoInstaller
    {
        public BaseAddEncounterPopup AddEncounterPopup { get => addEncounterPopup; set => addEncounterPopup = value; }
        [SerializeField] private BaseAddEncounterPopup addEncounterPopup;
        public BaseMessageHandler MessageHandler { get => messageHandler; set => messageHandler = value; }
        [SerializeField] private BaseMessageHandler messageHandler;
        public BaseMetadataSelector MetadataSelector { get => metadataSelector; set => metadataSelector = value; }
        [SerializeField] private BaseMetadataSelector metadataSelector;
        public AndroidBackButton BackButton { get => backButton; set => backButton = value; }
        [SerializeField] private AndroidBackButton backButton;
        public MainMenuEncounterUI EncounterSelectorPrefab { get => encounterSelectorPrefab; set => encounterSelectorPrefab = value; }
        [SerializeField] private MainMenuEncounterUI encounterSelectorPrefab;
        public BaseMenuEncounterOverview MenuEncounterOverview { get => menuEncounterOverview; set => menuEncounterOverview = value; }
        [SerializeField] private BaseMenuEncounterOverview menuEncounterOverview;
        public Transform PoolParent { get => poolParent; set => poolParent = value; }
        [SerializeField] private Transform poolParent;

        public override void InstallBindings()
        {
            Container.Bind<IMenuEncounterStarter>().To<MenuEncounterEditStarter>().AsTransient().WhenInjectedInto<EncounterSelectorWriterButtons>();
            Container.Bind<IMenuEncounterStarter>().To<MenuEncounterReadStarter>().AsTransient().WhenInjectedInto<EncounterSelectorReaderButtons>();

            Container.BindInterfacesTo<LoadingMenuSceneInfoSelector>().AsSingle();
            Container.BindInterfacesTo<MenuSceneInfoSelector>().AsSingle();

            Container.BindInstance(BackButton);
            Container.BindInstance(AddEncounterPopup);
            Container.BindInstance(MessageHandler);
            Container.BindInstance(MetadataSelector);
            Container.BindInstance(MenuEncounterOverview);

            Container.BindMemoryPool<MainMenuEncounterUI, MenuEncounterSelector.Pool>()
                .FromComponentInNewPrefab(EncounterSelectorPrefab)
                .UnderTransform(PoolParent);
        }
    }
}