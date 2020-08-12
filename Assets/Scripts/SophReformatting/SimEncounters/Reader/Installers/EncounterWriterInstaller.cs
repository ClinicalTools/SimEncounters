using ClinicalTools.SimEncounters.Data;
using System;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class EncounterWriterInstaller : MonoInstaller
    {
        protected FileManagerInstaller FileManagerInstaller { get; set; }

        public override void InstallBindings()
        {
            FileManagerInstaller = new FileManagerInstaller();
            InstallEncounterWriterBindings(Container);
        }

        protected virtual void InstallEncounterWriterBindings(DiContainer subcontainer)
        {
            foreach (SaveType saveType in Enum.GetValues(typeof(SaveType))) {
                subcontainer.Bind<IEncounterWriter>().WithId(saveType)
                    .FromSubContainerResolve().ByMethod(
                        (container) => BindEncounterWriterInstaller(container, saveType)).AsTransient();
            }
        }

        protected virtual void BindEncounterWriterInstaller(DiContainer subcontainer, SaveType saveType)
        {
            subcontainer.Bind<IEncounterWriter>().To<EncounterWriter>().AsTransient().WhenNotInjectedInto<EncounterWriter>();
            if (saveType == SaveType.Server) {
                subcontainer.Bind<IEncounterWriter>().To<EncounterUploader>().AsTransient().WhenInjectedInto<EncounterWriter>();
            } else {
                subcontainer.Bind<IEncounterWriter>().To<LocalEncounterWriter>().AsTransient().WhenInjectedInto<EncounterWriter>();
                subcontainer.Bind<IMetadataWriter>().To<LocalMetadataWriter>().AsTransient();
                FileManagerInstaller.BindFileManager(subcontainer, saveType);
            }
        }
    }
}