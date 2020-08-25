using System;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class FileManagerInstaller
    {
        public void BindFileManagerWithId(DiContainer subcontainer, SaveType saveType)
            => subcontainer.Bind<IFileManager>().WithId(saveType).FromSubContainerResolve().ByMethod(GetFileManagerInstaller(saveType)).AsTransient();
        public void BindFileManager(DiContainer subcontainer, SaveType saveType)
            => subcontainer.Bind<IFileManager>().FromSubContainerResolve().ByMethod(GetFileManagerInstaller(saveType)).AsTransient();

        protected virtual  Action<DiContainer> GetFileManagerInstaller(SaveType saveType)
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
                    UnityEngine.Debug.LogError("FileManager cannot be created for a server save type.");
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