using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IEncounterStarter>().To<EncounterEditStarter>().AsTransient().WhenInjectedInto<EncounterSelectorWriterButtons>();
            Container.Bind<IEncounterStarter>().To<EncounterReadStarter>().AsTransient().WhenInjectedInto<EncounterSelectorReaderButtons>();
            Container.Bind<IEncounterCopier>().To<EncounterCopier>().AsTransient();
        }
    }
}