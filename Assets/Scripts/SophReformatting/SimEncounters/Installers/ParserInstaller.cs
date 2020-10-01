using ClinicalTools.ClinicalEncounters;
using ClinicalTools.SimEncounters.Loading;
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
            subcontainer.Bind<IStringDeserializer<List<EncounterMetadata>>>().To<ListDeserializer<EncounterMetadata>>().AsTransient();
            subcontainer.Bind<IStringDeserializer<EncounterMetadata>>().To<BestCEEncounterMetadataDeserializer>().AsTransient()
                    .WhenNotInjectedInto<CEEncounterMetadataDeserializer>();
            subcontainer.Bind<IStringDeserializer<EncounterMetadata>>().To<LegacyCEEncounterMetadataParser>().AsTransient()
                    .WhenInjectedInto<CEEncounterMetadataDeserializer>();

            subcontainer.Bind<IStringDeserializer<Dictionary<int, EncounterBasicStatus>>>().To<DictionaryDeserializer<int, EncounterBasicStatus>>().AsTransient();
            subcontainer.Bind<IStringDeserializer<KeyValuePair<int, EncounterBasicStatus>>>().To<KeyedEncounterStatusDeserializer>().AsTransient();

            subcontainer.Bind<IStringSerializer<KeyValuePair<int, EncounterBasicStatus>>>().To<KeyedEncounterStatusSerializer>().AsTransient();

            subcontainer.Bind<IStringDeserializer<EncounterNonImageContent>>().To<XmlStringDeserializer<EncounterNonImageContent>>().AsTransient();
            subcontainer.Bind<IStringDeserializer<EncounterImageContent>>().To<XmlStringDeserializer<EncounterImageContent>>().AsTransient();
            subcontainer.Bind<IStringDeserializer<XmlDocument>>().To<XmlDocumentDeserializer>().AsTransient();

            subcontainer.Bind<IStringDeserializer<EncounterContentStatus>>().To<EncounterContentStatusDeserializer>().AsTransient();
            subcontainer.Bind<ICharEnumeratorDeserializer<SectionStatus>>().To<SectionStatusDeserializer>().AsTransient();
            subcontainer.Bind<ICharEnumeratorDeserializer<TabStatus>>().To<TabStatusDeserializer>().AsTransient();
            subcontainer.Bind<ICharEnumeratorDeserializer<string>>().To<KeyDeserializer>().AsTransient();

            subcontainer.Bind<IStringDeserializer<Color>>().To<ColorDeserializer>().AsTransient();

            subcontainer.Bind<IStringSerializer<Sprite>>().To<SpriteSerializer>().AsTransient();
        }

        /// <summary>
        /// IL2CPP will strip generic constructors, so they have to be referenced in order to not be stripped.
        /// https://github.com/svermeulen/Extenject#aot-support
        /// </summary>
        public static void ForceIL2CPPToKeepNeededGenericConstructors()
        {
            new DictionaryDeserializer<int, EncounterBasicStatus>(null, null);
            new ListDeserializer<EncounterMetadata>(null, null);
            new XmlStringDeserializer<EncounterNonImageContent>(null, null);
            new XmlStringDeserializer<EncounterImageContent>(null, null);
        }
    }
}