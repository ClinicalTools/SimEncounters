namespace ClinicalTools.SimEncounters
{
    public class ReaderPatientImage : ReaderImage
    {

        private const string patientImageKey = "patientImage";
        public override void Initialize(Encounter encounter) => Initialize(encounter, patientImageKey);
    }
}