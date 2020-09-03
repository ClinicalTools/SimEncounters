namespace ClinicalTools.SimEncounters
{
    public class SectionStatusSerializer
    {
        private const char END_CHAR = ' ';
        private readonly TabStatusSerializer tabStatusSerializer;
        public SectionStatusSerializer(TabStatusSerializer tabStatusSerializer)
        {
            this.tabStatusSerializer = tabStatusSerializer;
        }

        public string Serialize(EncounterContentStatus encounterStatus, SectionStatus sectionStatus)
        {
            var str = "";
            foreach (var tab in sectionStatus.TabStatuses) {
                var tabStr = tabStatusSerializer.Serialize(sectionStatus, tab.Value);
                if (!string.IsNullOrWhiteSpace(tabStr))
                    str += tab.Key + tabStr + END_CHAR;
            }

            if (str.Length == 0 && sectionStatus.Read == encounterStatus.Read)
                return null;

            var readChar = sectionStatus.Read ? '1' : '0';
            str = readChar + str;

            return str;
        }
    }
}