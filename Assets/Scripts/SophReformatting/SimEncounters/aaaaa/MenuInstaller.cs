using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MenuInstaller : MonoInstaller
    {
        public AddEncounterPopup AddEncounterPopup { get => addEncounterPopup; set => addEncounterPopup = value; }
        [SerializeField] private AddEncounterPopup addEncounterPopup;

        public override void InstallBindings()
        {
            Container.Bind<IEncounterStarter>().To<EncounterEditStarter>().AsTransient().WhenInjectedInto<EncounterSelectorWriterButtons>();
            Container.Bind<IEncounterStarter>().To<EncounterReadStarter>().AsTransient().WhenInjectedInto<EncounterSelectorReaderButtons>();
            Container.BindInstance(AddEncounterPopup);
        }
    }
}