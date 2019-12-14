using ClinicalTools.SimEncounters.Collections;

namespace ClinicalTools.SimEncounters.Data
{
    public class SectionsData
    {
        public virtual OrderedCollection<Section> Sections { get; } = new OrderedCollection<Section>();
        public virtual VariableData Variables { get; }

        public SectionsData() {
            Variables = new VariableData();
        }

        public SectionsData(VariableData variables)
        {
            Variables = variables;
        }
    }
}