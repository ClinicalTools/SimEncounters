using ClinicalTools.SimEncounters.Data;
using System;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterReaderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallBasicServerBindings(Container);
            InstallMenuReaderBindings(Container);
            InstallEncounterReaderBindings(Container);
        }
        protected virtual void InstallBasicServerBindings(DiContainer subcontainer)
        {
            subcontainer.Bind<IUrlBuilder>().To<UrlBuilder>().AsTransient();
            subcontainer.Bind<IServerReader>().To<ServerReader>().AsTransient();
        }

        protected virtual void InstallMenuReaderBindings(DiContainer subcontainer)
        {
            subcontainer.Bind<IMenuEncountersInfoReader>().To<MenuEncountersInfoReader>().AsTransient();
            subcontainer.Bind<IMenuEncountersReader>().To<MenuEncountersReader>().AsTransient();
            InstallMetadataReaderBindings(subcontainer);

            subcontainer.Bind<IBasicStatusesReader>().To<BasicStatusesReader>().AsTransient().WhenNotInjectedInto<BasicStatusesReader>();
            subcontainer.Bind<IBasicStatusesReader>().To<LocalBasicStatusesReader>().AsTransient().WhenInjectedInto<BasicStatusesReader>();
            subcontainer.Bind<IBasicStatusesReader>().To<ServerBasicStatusesReader>().AsTransient().WhenInjectedInto<BasicStatusesReader>();
            subcontainer.Bind<IFileManager>().FromSubContainerResolve().ByMethod(GetFileManagerInstaller(SaveType.Local)).AsTransient();
        }

        protected virtual void InstallMetadataReaderBindings(DiContainer subcontainer)
        {
            subcontainer.Bind<IMetadataGroupsReader>().To<MetadataGroupsReader>().AsTransient();
            foreach (SaveType saveType in Enum.GetValues(typeof(SaveType))) {
                subcontainer.Bind<IMetadatasReader>().WithId(saveType)
                    .FromSubContainerResolve().ByMethod((container) => BindMetadatasReaderInstaller(container, saveType)).AsTransient();
            }
        }
        protected virtual void BindMetadatasReaderInstaller(DiContainer subcontainer, SaveType saveType)
        {
            if (saveType == SaveType.Server) {
                subcontainer.Bind<IMetadatasReader>().To<ServerMetadatasReader>().AsTransient();
            } else {
                subcontainer.Bind<IMetadatasReader>().To<LocalMetadatasReader>().AsTransient();
                subcontainer.Bind<IFileManager>().FromSubContainerResolve().ByMethod(GetFileManagerInstaller(saveType)).AsTransient();
            }
        }

        protected virtual void InstallEncounterReaderBindings(DiContainer subcontainer)
        {
            subcontainer.Bind<IUserEncounterReader>().To<UserEncounterReader>().AsTransient();
            subcontainer.Bind<IDetailedStatusReader>().To<LocalDetailedStatusReader>().AsTransient();
            subcontainer.Bind<IEncounterReader>().To<CEEncounterReader>().AsTransient();
            InstallEncounterDataReaderBindings(subcontainer);
        }

        protected virtual void InstallEncounterDataReaderBindings(DiContainer subcontainer)
        {
            subcontainer.Bind<IEncounterDataReaderSelector>().To<EncounterDataReaderSelector>().AsTransient();
            foreach (SaveType saveType in Enum.GetValues(typeof(SaveType))) {
                subcontainer.Bind<IEncounterDataReader>().WithId(saveType)
                    .FromSubContainerResolve().ByMethod((container) => BindEncounterDataReaderInstaller(container, saveType)).AsTransient();
            }
        }

        protected virtual void BindEncounterDataReaderInstaller(DiContainer subcontainer, SaveType saveType)
        {
            subcontainer.Bind<IEncounterDataReader>().To<EncounterDataReader>().AsTransient();
            if (saveType == SaveType.Server) {
                subcontainer.Bind<IEncounterContentReader>().To<ServerContentDataReader>().AsTransient();
                subcontainer.Bind<IImageDataReader>().To<ServerImageDataReader>().AsTransient();
            } else {
                subcontainer.Bind<IEncounterContentReader>().To<LocalContentDataReader>().AsTransient();
                subcontainer.Bind<IImageDataReader>().To<LocalImageDataReader>().AsTransient();
                subcontainer.Bind<IFileManager>().FromSubContainerResolve().ByMethod(GetFileManagerInstaller(saveType)).AsTransient();
            }
        }

        protected virtual Action<DiContainer> GetFileManagerInstaller(SaveType saveType)
        {
            switch (saveType) {
                case SaveType.Default:
                    return BindFileManager<DefaultFileManager, FileExtensionManager>;
                case SaveType.Autosave:
                    return BindFileManager<UserFileManager, AutosaveFileExtensionManager>;
                case SaveType.Demo:
                    return BindFileManager<DemoFileManager, FileExtensionManager>;
                case SaveType.Local:
                    return BindFileManager<UserFileManager, FileExtensionManager>;
                default:
                    return null;
            }
        }

        protected virtual void BindFileManager<TFileManager, TFileExtensionManager>(DiContainer subcontainer)
            where TFileManager : IFileManager
            where TFileExtensionManager : IFileExtensionManager
        {
            subcontainer.Bind<IFileManager>().To<TFileManager>().AsTransient();
            subcontainer.Bind<IFileExtensionManager>().To<TFileExtensionManager>().AsTransient();
        }
    }
}