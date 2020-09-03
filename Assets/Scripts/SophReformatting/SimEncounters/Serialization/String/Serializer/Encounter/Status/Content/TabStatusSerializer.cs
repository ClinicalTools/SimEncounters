namespace ClinicalTools.SimEncounters
{
    public class TabStatusSerializer
    {
        public string Serialize(SectionStatus section, TabStatus tab)
        {
            if (tab.Read == section.Read)
                return null;

            var str = "";
            str += tab.Read ? '1' : '0';

            return str;
        }
    }
}