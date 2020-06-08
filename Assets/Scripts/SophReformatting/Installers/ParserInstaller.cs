using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loading;
using ClinicalTools.SimEncounters.MainMenu;
using System.Collections.Generic;
using System.Xml;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ParserInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallParserBindings(Container);
        }

        protected virtual void InstallParserBindings(DiContainer subcontainer)
        {
            subcontainer.Bind<IStringSplitter>().To<DoubleColonStringSplitter>().AsTransient();
            subcontainer.Bind<IParser<List<EncounterMetadata>>>().To<ListParser<EncounterMetadata>>().AsTransient();
            //EncounterMetadataDeserializer
            subcontainer.Bind<IParser<EncounterMetadata>>().To<EncounterMetadataDeserializer>().AsTransient()
                    .WhenNotInjectedInto<EncounterMetadataDeserializer>();
            subcontainer.Bind<IParser<EncounterMetadata>>().To<EncounterMetadataParser>().AsTransient()
                    .WhenInjectedInto<EncounterMetadataDeserializer>();

            subcontainer.Bind<IParser<Dictionary<int, EncounterBasicStatus>>>().To<DictionaryParser<int, EncounterBasicStatus>>().AsTransient();
            subcontainer.Bind<IParser<KeyValuePair<int, EncounterBasicStatus>>>().To<KeyedEncounterStatusParser>().AsTransient();

            subcontainer.Bind<IParser<EncounterContent>>().To<XmlDeserializerParser<EncounterContent>>().AsTransient();
            subcontainer.Bind<IParser<EncounterImageData>>().To<XmlDeserializerParser<EncounterImageData>>().AsTransient();
            subcontainer.Bind<IParser<XmlDocument>>().To<XmlParser>().AsTransient();

            subcontainer.Bind<IParser<EncounterContentStatus>>().To<EncounterContentStatusParser>().AsTransient();
            subcontainer.Bind<ICharEnumeratorParser<SectionStatus>>().To<SectionStatusParser>().AsTransient();
            subcontainer.Bind<ICharEnumeratorParser<TabStatus>>().To<TabStatusParser>().AsTransient();
            subcontainer.Bind<ICharEnumeratorParser<string>>().To<KeyParser>().AsTransient();
        }
    }
}