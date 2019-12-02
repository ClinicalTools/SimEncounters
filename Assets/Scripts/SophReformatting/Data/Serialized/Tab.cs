using SimEncounters.Xml;
using System.Collections.Generic;


namespace SimEncounters.Data
{
    public class Tab
    {
        public virtual string Type { get; }
        public virtual string Name { get; set; } //Display name, not formatted
        public virtual bool Persistent { get; } //If true, cannot change
        public virtual List<string> Conditions { get; set; } = new List<string>();
        public virtual List<Panel> Panels { get; set; }


        public Tab(string type)
        {
            Type = type;
            Name = type;
        }

        /**
         * Data script to hold info for every tab. Stored in SectionDataScript.Dict
         */
        public Tab(string type, string name, bool persistent)
        {
            Type = type;
            Name = name;
            Persistent = persistent;
        }
    }
}