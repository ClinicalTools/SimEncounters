using ClinicalTools.SimEncounters.Collections;

namespace ClinicalTools.SimEncounters.Data
{
    public class Tab
    {
        public virtual string Type { get; }
        public virtual string Name { get; set; }
        public virtual ConditionalData Conditions { get; set; }
        public virtual OrderedCollection<Panel> Panels { get; } = new OrderedCollection<Panel>();


        public Tab(string type)
        {
            Type = type;
            Name = type;
        }

        /**
         * Data script to hold info for every tab. Stored in SectionDataScript.Dict
         */
        public Tab(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}