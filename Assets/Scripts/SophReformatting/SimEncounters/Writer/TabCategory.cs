using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabCategory
    {
        public string Name { get; }
        public List<TabType> Types { get; } = new List<TabType>();

        public TabCategory(string name)
        {
            Name = name;
        }
    }
}