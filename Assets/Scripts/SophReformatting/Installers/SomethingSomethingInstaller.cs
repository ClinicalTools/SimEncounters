using ClinicalTools.SimEncounters.Data;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SomethingSomethingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IEncounterDataReaderSelector>().To<EncounterDataReaderSelector>().AsTransient();
            Container.Bind<IEncounterReader>().To<CEEncounterReader>().AsTransient();

            Container.Bind<IEncounterDataReader>().FromSubContainerResolve()
        }
        protected virtual void Server(DiContainer subcontainer)
        {
            subcontainer.Bind<IUrlBuilder>().To<UrlBuilder>().AsTransient();
            subcontainer.Bind<IServerReader>().To<ServerReader>().AsTransient();
        }
        protected virtual void ServerSomething(DiContainer subcontainer)
        {
            Container.Bind<IEncounterContentReader>().To<ServerContentDataReader>().AsTransient();
            Container.Bind<IImageDataReader>().To<ServerImageDataReader>().AsTransient();
        }
        protected virtual void General<TFileManager, TFileExtensionManager>(DiContainer subcontainer)
            where TFileManager : IFileManager
            where TFileExtensionManager : IFileExtensionManager
        {
            Container.Bind<IFileManager>().To<TFileManager>().AsTransient();
            Container.Bind<IFileExtensionManager>().To<TFileExtensionManager>().AsTransient();
            subcontainer.Bind<IEncounterContentReader>().To<LocalContentDataReader>().AsTransient();
            subcontainer.Bind<IImageDataReader>().To<LocalImageDataReader>().AsTransient();
        }
        protected virtual void LocalSomething(DiContainer subcontainer)
            => General<UserFileManager, FileExtensionManager>(subcontainer);
        protected virtual void AutosaveSomething(DiContainer subcontainer)
            => General<UserFileManager, AutosaveFileExtensionManager>(subcontainer);
        protected virtual void DemoSomething(DiContainer subcontainer)
            => General<DemoFileManager, FileExtensionManager>(subcontainer);
        protected virtual void DefaultSomething(DiContainer subcontainer)
            => General<DefaultFileManager, FileExtensionManager>(subcontainer);
    }
}