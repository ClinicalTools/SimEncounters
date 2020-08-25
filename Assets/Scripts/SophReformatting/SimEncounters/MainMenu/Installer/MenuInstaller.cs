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

        public override void InstallBindings()
        {
            Application.logMessageReceived += Logger.Application_logMessageReceived;
            Container.Bind<IEncounterStarter>().To<EncounterEditStarter>().AsTransient().WhenInjectedInto<EncounterSelectorWriterButtons>();
            Container.Bind<IEncounterStarter>().To<EncounterReadStarter>().AsTransient().WhenInjectedInto<EncounterSelectorReaderButtons>();
            Container.BindInstance(AddEncounterPopup);
            Container.BindInstance(MessageHandler);
            Container.BindInstance(MetadataSelector);
        }
    }
}