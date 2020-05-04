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
            Container.Bind<ISerializationFactory<EncounterImageData>>().To<ImageDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Sprite>>().To<SpriteFactory>().AsTransient();
            Container.Bind<ISerializationFactory<LegacyIcon>>().To<IconFactory>().AsTransient();

            Container.Bind<ISerializationFactory<EncounterContent>>().To<EncounterDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Section>>().To<SectionFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Tab>>().To<TabFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Panel>>().To<PanelFactory>().AsTransient();

            Container.Bind<ISerializationFactory<PinData>>().To<PinDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<DialoguePin>>().To<DialoguePinFactory>().AsTransient();
            Container.Bind<ISerializationFactory<QuizPin>>().To<QuizPinFactory>().AsTransient();

            Container.Bind<ISerializationFactory<ConditionalData>>().To<ConditionalDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<BoolConditional>>().To<BoolConditionalFactory>().AsTransient();
            Container.Bind<ISerializationFactory<IntConditional>>().To<IntConditionalFactory>().AsTransient();

            Container.Bind<ISerializationFactory<VariableData>>().To<VariableDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<EncounterBool>>().To<EncounterBoolFactory>().AsTransient();
            Container.Bind<ISerializationFactory<EncounterInt>>().To<EncounterIntFactory>().AsTransient();
        }
    }
}