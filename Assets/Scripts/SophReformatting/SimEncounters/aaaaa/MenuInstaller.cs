using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MenuInstaller : MonoInstaller
    {
        public BaseAddEncounterPopup AddEncounterPopup { get => addEncounterPopup; set => addEncounterPopup = value; }
        [SerializeField] private BaseAddEncounterPopup addEncounterPopup;
        public BaseMessageHandler MessageHandler { get => messageHandler; set => messageHandler = value; }
        [SerializeField] private BaseMessageHandler messageHandler;

        public override void InstallBindings()
        {
            Application.logMessageReceived += Aaaa.Application_logMessageReceived;
            Container.Bind<IEncounterStarter>().To<EncounterEditStarter>().AsTransient().WhenInjectedInto<EncounterSelectorWriterButtons>();
            Container.Bind<IEncounterStarter>().To<EncounterReadStarter>().AsTransient().WhenInjectedInto<EncounterSelectorReaderButtons>();
            Container.BindInstance(AddEncounterPopup);
            Container.BindInstance(MessageHandler);
        }
    }
}