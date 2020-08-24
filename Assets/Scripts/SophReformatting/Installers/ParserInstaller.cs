using ClinicalTools.ClinicalEncounters;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loading;
using ClinicalTools.SimEncounters.MainMenu;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ParserInstaller : MonoInstaller
    {
        public override void InstallBindings() => InstallParserBindings(Container);

        protected virtual void InstallParserBindings(DiContainer subcontainer)
        {
            subcontainer.Bind<IStringSplitter>().To<DoubleColonStringSplitter>().AsTransient();
            subcontainer.Bind<IParser<List<EncounterMetadata>>>().To<ListParser<EncounterMetadata>>().AsTransient();
            subcontainer.Bind<IParser<EncounterMetadata>>().To<BestCEEncounterMetadataDeserializer>().AsTransient()
                    .WhenNotInjectedInto<CEEncounterMetadataDeserializer>();
            subcontainer.Bind<IParser<EncounterMetadata>>().To<LegacyCEEncounterMetadataParser>().AsTransient()
                    .WhenInjectedInto<CEEncounterMetadataDeserializer>();

            subcontainer.Bind<IParser<Dictionary<int, EncounterBasicStatus>>>().To<DictionaryParser<int, EncounterBasicStatus>>().AsTransient();
            subcontainer.Bind<IParser<KeyValuePair<int, EncounterBasicStatus>>>().To<KeyedEncounterStatusParser>().AsTransient();

            subcontainer.Bind<IParser<EncounterNonImageContent>>().To<XmlDeserializerParser<EncounterNonImageContent>>().AsTransient();
            subcontainer.Bind<IParser<EncounterImageContent>>().To<XmlDeserializerParser<EncounterImageContent>>().AsTransient();
            subcontainer.Bind<IParser<XmlDocument>>().To<XmlParser>().AsTransient();

            subcontainer.Bind<IParser<EncounterContentStatus>>().To<EncounterContentStatusParser>().AsTransient();
            subcontainer.Bind<ICharEnumeratorParser<SectionStatus>>().To<SectionStatusParser>().AsTransient();
            subcontainer.Bind<ICharEnumeratorParser<TabStatus>>().To<TabStatusParser>().AsTransient();
            subcontainer.Bind<ICharEnumeratorParser<string>>().To<KeyParser>().AsTransient();

            subcontainer.Bind<IParser<Color>>().To<ColorParser>().AsTransient();
        }

        /// <summary>
        /// IL2CPP will strip generic constructors, so they have to be referenced in order to not be stripped.
        /// https://github.com/svermeulen/Extenject#aot-support
        /// </summary>
        public static void ForceIL2CPPToKeepNeededGenericConstructors()
        {
            new DictionaryParser<int, EncounterBasicStatus>(null, null);
            new ListParser<EncounterMetadata>(null, null);
            new XmlDeserializerParser<EncounterNonImageContent>(null, null);
            new XmlDeserializerParser<EncounterImageContent>(null, null);
        }
    }
}