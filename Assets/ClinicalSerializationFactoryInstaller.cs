using ClinicalTools.ClinicalEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;
using UnityEngine;
using Zenject;

namespace ClinicalTools.ClinicalEncounters
{
    public class ClinicalSerializationFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISerializationFactory<EncounterImageData>>().To<ClinicalImageDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Sprite>>().To<ClinicalSpriteFactory>().AsTransient();
            Container.Bind<ISerializationFactory<LegacyIcon>>().To<ClinicalIconFactory>().AsTransient();

            Container.Bind<ISerializationFactory<EncounterContent>>().To<ClinicalEncounterDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Section>>().To<ClinicalSectionFactory>().AsTransient();
            // Clinical Sections don't have proper keys, so they need to be regenerated. 
            Container.Bind<IKeyGenerator>().To<KeyGenerator>().AsTransient().WithArguments(0);
            Container.Bind<ISerializationFactory<Tab>>().To<ClinicalTabFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Panel>>().To<ClinicalPanelFactory>().AsTransient();

            Container.Bind<ISerializationFactory<PinData>>().To<ClinicalPinDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<DialoguePin>>().To<ClinicalDialoguePinFactory>().AsTransient();
            Container.Bind<ISerializationFactory<QuizPin>>().To<ClinicalQuizPinFactory>().AsTransient();

            Container.Bind<ISerializationFactory<ConditionalData>>().To<ConditionalDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<BoolConditional>>().To<BoolConditionalFactory>().AsTransient();
            Container.Bind<ISerializationFactory<IntConditional>>().To<IntConditionalFactory>().AsTransient();

            Container.Bind<ISerializationFactory<VariableData>>().To<VariableDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<EncounterBool>>().To<EncounterBoolFactory>().AsTransient();
            Container.Bind<ISerializationFactory<EncounterInt>>().To<EncounterIntFactory>().AsTransient();
        }
    }
}
