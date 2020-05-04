using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class TabStatusSerializer
    {
        public string Serialize(SectionStatus section, TabStatus tab)
        {
            if (tab.Read == section.Read)
                return null;

            var str = "";
            if (tab.Read)
                str += '1';
            else
                str += '0';

            return str;
        }
    }
}