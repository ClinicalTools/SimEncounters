using ClinicalTools.SimEncounters;
using Zenject;

namespace ClinicalTools.ClinicalEncounters
{
    public class CEStringSerializingInstaller : StringSerializingInstaller
    {
        protected override void BindMetadataDeserializer(DiContainer subcontainer)
            => subcontainer.Bind<IStringDeserializer<EncounterMetadata>>()
                           .To<CEEncounterMetadataDeserializer>()
                           .AsTransient();
    }
}