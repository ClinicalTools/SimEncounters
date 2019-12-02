using System.Collections.Generic;

namespace SimEncounters.Data
{
    public class NewEncounterData
    {
        public virtual List<Section> Sections { get; } = new List<Section>();
        public virtual KeyedCollection<EncounterBool> Bools { get; } = new KeyedCollection<EncounterBool>();
        public virtual KeyedCollection<EncounterInt> Ints { get; } = new KeyedCollection<EncounterInt>();

        public NewEncounterData() { }
        public NewEncounterData(List<Section> sections, KeyedCollection<EncounterBool> bools, KeyedCollection<EncounterInt> ints)
        {
            Sections = sections;
            Bools = bools;
            Ints = ints;
        }
    }
}