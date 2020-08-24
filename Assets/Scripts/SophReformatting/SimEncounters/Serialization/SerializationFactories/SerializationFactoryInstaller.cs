using ClinicalTools.ClinicalEncounters.Data;
using ClinicalTools.ClinicalEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SerializationFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISerializationFactory<EncounterImageContent>>().To<ImageDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Sprite>>().To<SpriteFactory>().AsTransient();
            Container.Bind<ISerializationFactory<LegacyIcon>>().To<IconFactory>().AsTransient();

            Container.Bind<ISerializationFactory<EncounterNonImageContent>>().To<EncounterDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Section>>().To<SectionFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Tab>>().To<TabFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Panel>>().To<PanelFactory>().AsTransient();

            Container.Bind<ISerializationFactory<PinData>>().To<PinDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<DialoguePin>>().To<DialoguePinFactory>().AsTransient();
            Container.Bind<ISerializationFactory<QuizPin>>().To<QuizPinFactory>().AsTransient();
        }
    }
}