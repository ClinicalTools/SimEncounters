using ClinicalTools.SimEncounters.Collections;

namespace ClinicalTools.SimEncounters.Data
{
    public class VariableData
    {
        public virtual KeyedCollection<EncounterBool> Bools { get; } = new KeyedCollection<EncounterBool>();
        public virtual KeyedCollection<EncounterInt> Ints { get; } = new KeyedCollection<EncounterInt>();

        public VariableData() { }
    }
}