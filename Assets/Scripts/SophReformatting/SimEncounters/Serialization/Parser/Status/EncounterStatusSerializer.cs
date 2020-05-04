using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class EncounterStatusSerializer
    {
        private const char END_CHAR = ' ';
        private readonly SectionStatusSerializer sectionStatusSerializer;
        public EncounterStatusSerializer(SectionStatusSerializer sectionStatusSerializer)
        {
            this.sectionStatusSerializer = sectionStatusSerializer;
        }

        public string Serialize(EncounterContentStatus encounterStatus)
        {
            var str = "";

            str += encounterStatus.Read ? '1' : '0';

            foreach (var section in encounterStatus.SectionStatuses) {
                var sectionStr = sectionStatusSerializer.Serialize(encounterStatus, section.Value);
                if (sectionStr != null)
                    str += section.Key + sectionStr + END_CHAR;
            }

            return str.Trim();
        }
    }
}