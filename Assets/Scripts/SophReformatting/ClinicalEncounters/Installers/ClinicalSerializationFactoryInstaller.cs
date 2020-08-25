using ClinicalTools.ClinicalEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.XmlSerialization;
using UnityEngine;
using Zenject;

namespace ClinicalTools.ClinicalEncounters
{
    public class ClinicalSerializationFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISerializationFactory<EncounterImageContent>>().To<ClinicalImageDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Sprite>>().To<ClinicalSpriteFactory>().AsTransient();
            Container.Bind<ISerializationFactory<LegacyIcon>>().To<ClinicalIconFactory>().AsTransient();

            Container.Bind<ISerializationFactory<EncounterNonImageContent>>().To<ClinicalEncounterDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Section>>().To<ClinicalSectionFactory>().AsTransient();
            // Clinical Sections don't have proper keys, so they need to be regenerated. 
            Container.Bind<IKeyGenerator>().To<KeyGenerator>().AsTransient().WithArguments(0);
            Container.Bind<ISerializationFactory<Tab>>().To<ClinicalTabFactory>().AsTransient();
            Container.Bind<ISerializationFactory<Panel>>().To<ClinicalPanelFactory>().AsTransient();

            Container.Bind<ISerializationFactory<PinData>>().To<ClinicalPinDataFactory>().AsTransient();
            Container.Bind<ISerializationFactory<DialoguePin>>().To<ClinicalDialoguePinFactory>().AsTransient();
            Container.Bind<ISerializationFactory<QuizPin>>().To<ClinicalQuizPinFactory>().AsTransient();
        }
    }
}
