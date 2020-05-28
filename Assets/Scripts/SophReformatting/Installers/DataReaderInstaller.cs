using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loading;
using ClinicalTools.SimEncounters.MainMenu;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class DataReaderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IStringSplitter>().To<DoubleColonStringSplitter>().AsTransient();

            Container.Bind<IEncounterReaderSelector>().To<EncounterReaderSelector>().AsTransient();
            Container.Bind<IUserEncounterReaderSelector>().To<UserEncounterReaderSelector>().AsTransient();
            Container.Bind<IUserEncounterReader>().To<UserEncounterReader>().AsTransient();
            Container.Bind<IEncounterReader>().To<CEEncounterReader>().AsTransient();

            Container.Bind<IDetailedStatusReader>().To<LocalDetailedStatusReader>().AsTransient();

            Container.Bind<IMenuEncountersInfoReader>().To<MenuEncountersInfoReader>().AsTransient();
            Container.Bind<IMenuEncountersReader>().To<MenuEncountersReader>().AsTransient();
            Container.Bind<IMetadataGroupsReader>().To<MetadataGroupsReader>().AsTransient();

            Container.Bind<IBasicStatusesReader>().To<BasicStatusesReader>().AsTransient().WhenNotInjectedInto<BasicStatusesReader>();
            Container.Bind<IBasicStatusesReader>().To<LocalBasicStatusesReader>().AsTransient().WhenInjectedInto<BasicStatusesReader>();
            Container.Bind<IBasicStatusesReader>().To<ServerBasicStatusesReader>().AsTransient().WhenInjectedInto<BasicStatusesReader>();

            InstallServerReaderBindings();
            InstallParserBindings();

            InstallLocalTypeBindings();
            InstallAutosaveTypeBindings();
            InstallDemoTypeBindings();
            InstallServerTypeBindings();
            InstallDefaultTypeBindings();
        }

        protected virtual void InstallServerReaderBindings()
        {
            Container.Bind<IUrlBuilder>().To<UrlBuilder>().AsTransient();
            Container.Bind<IServerReader>().To<ServerReader>().AsTransient();
        }
        protected virtual void InstallParserBindings()
        {
            Container.Bind<IParser<List<EncounterMetadata>>>().To<ListParser<EncounterMetadata>>().AsTransient();
            Container.Bind<IParser<EncounterMetadata>>().To<EncounterMetadataParser>().AsTransient();

            Container.Bind<IParser<Dictionary<int, EncounterBasicStatus>>>().To<DictionaryParser<int, EncounterBasicStatus>>().AsTransient();
            Container.Bind<IParser<KeyValuePair<int, EncounterBasicStatus>>>().To<KeyedEncounterStatusParser>().AsTransient();

            Container.Bind<IParser<EncounterContent>>().To<XmlDeserializerParser<EncounterContent>>().AsTransient();
            Container.Bind<IParser<EncounterImageData>>().To<XmlDeserializerParser<EncounterImageData>>().AsTransient();
            Container.Bind<IParser<XmlDocument>>().To<XmlParser>().AsTransient();

            Container.Bind<IParser<EncounterContentStatus>>().To<EncounterContentStatusParser>().AsTransient();
            Container.Bind<ICharEnumeratorParser<SectionStatus>>().To<SectionStatusParser>().AsTransient();
            Container.Bind<ICharEnumeratorParser<TabStatus>>().To<TabStatusParser>().AsTransient();
            Container.Bind<ICharEnumeratorParser<string>>().To<KeyParser>().AsTransient();
        }

        protected virtual void InstallServerTypeBindings()
        {
            bool serverBindingCondition(InjectContext injectContext) => ContextHierarchyContainsIdentifier(injectContext, SaveType.Server);

            Container.Bind<IUserEncounterReader>().WithId(SaveType.Server).To<UserEncounterReader>().AsTransient();
            Container.Bind<IMetadatasReader>().WithId(SaveType.Server).To<ServerMetadatasReader>().AsTransient();
            Container.Bind<IEncounterReader>().WithId(SaveType.Server).To<CEEncounterReader>().AsTransient();

            Container.Bind<IEncounterContentReader>().To<ServerContentDataReader>().AsTransient().When(serverBindingCondition);
            Container.Bind<IImageDataReader>().To<ServerImageDataReader>().AsTransient().When(serverBindingCondition);

            Container.Bind<IFileManager>().To<UserFileManager>().AsTransient().When(serverBindingCondition);
            Container.Bind<IFileExtensionManager>().To<FileExtensionManager>().AsTransient().When(serverBindingCondition);
        }
        protected virtual void InstallLocalBindings(BindingCondition bindingCondition)
        {
            Container.Bind<IEncounterContentReader>().To<LocalContentDataReader>().AsTransient().When(bindingCondition);
            Container.Bind<IImageDataReader>().To<LocalImageDataReader>().AsTransient().When(bindingCondition);
        }

        protected virtual void InstallDefaultTypeBindings()
        {
            Container.Bind<IFileManager>().To<UserFileManager>().AsTransient().When(ContextHierarchyContainsNoIdentifier);
            Container.Bind<IFileExtensionManager>().To<FileExtensionManager>().AsTransient().When(ContextHierarchyContainsNoIdentifier);
        }

        protected virtual void InstallLocalTypeBindings()
        {
            Container.Bind<IMetadatasReader>().WithId(SaveType.Local).To<LocalMetadatasReader>().AsTransient();
            Container.Bind<IUserEncounterReader>().WithId(SaveType.Local).To<UserEncounterReader>().AsTransient();
            Container.Bind<IEncounterReader>().WithId(SaveType.Local).To<CEEncounterReader>().AsTransient();

            bool localBindingCondition(InjectContext injectContext) => ContextHierarchyContainsIdentifier(injectContext, SaveType.Local);

            Container.Bind<IFileManager>().To<UserFileManager>().AsTransient().When(localBindingCondition);
            Container.Bind<IFileExtensionManager>().To<FileExtensionManager>().AsTransient().When(localBindingCondition);

            InstallLocalBindings(localBindingCondition);
        }


        protected virtual void InstallAutosaveTypeBindings()
        {
            Container.Bind<IMetadatasReader>().WithId(SaveType.Autosave).To<LocalMetadatasReader>().AsTransient();
            Container.Bind<IUserEncounterReader>().WithId(SaveType.Autosave).To<UserEncounterReader>().AsTransient();
            Container.Bind<IEncounterReader>().WithId(SaveType.Autosave).To<CEEncounterReader>().AsTransient();

            bool autosaveBindingCondition(InjectContext injectContext) => ContextHierarchyContainsIdentifier(injectContext, SaveType.Autosave);

            Container.Bind<IFileManager>().To<UserFileManager>().AsTransient().When(autosaveBindingCondition);
            Container.Bind<IFileExtensionManager>().To<AutosaveFileExtensionManager>().AsTransient().When(autosaveBindingCondition);

            InstallLocalBindings(autosaveBindingCondition);
        }

        protected virtual void InstallDemoTypeBindings()
        {
            Container.Bind<IMetadatasReader>().WithId(SaveType.Demo).To<LocalMetadatasReader>().AsTransient();
            Container.Bind<IUserEncounterReader>().WithId(SaveType.Demo).To<UserEncounterReader>().AsTransient();
            Container.Bind<IEncounterReader>().WithId(SaveType.Demo).To<CEEncounterReader>().AsTransient();

            bool demoBindingCondition(InjectContext injectContext) => ContextHierarchyContainsIdentifier(injectContext, SaveType.Demo);

            Container.Bind<IFileManager>().To<DemoFileManager>().AsCached().When(demoBindingCondition);
            Container.Bind<IFileExtensionManager>().To<FileExtensionManager>().AsTransient().When(demoBindingCondition);

            InstallLocalBindings(demoBindingCondition);
        }

        protected virtual bool ContextHierarchyContainsIdentifier(InjectContext injectContext, SaveType identifier)
        {
            if (injectContext.Identifier is SaveType && (SaveType)injectContext.Identifier == identifier)
                return true;

            if (injectContext.ParentContext != null)
                return ContextHierarchyContainsIdentifier(injectContext.ParentContext, identifier);

            return false;
        }

        protected virtual bool ContextHierarchyContainsNoIdentifier(InjectContext injectContext)
        {
            if (injectContext.Identifier != null)
                return false;

            if (injectContext.ParentContext != null)
                return ContextHierarchyContainsNoIdentifier(injectContext.ParentContext);

            return true;
        }
    }
}