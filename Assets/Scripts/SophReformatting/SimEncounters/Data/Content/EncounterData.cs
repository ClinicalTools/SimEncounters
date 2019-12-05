namespace ClinicalTools.SimEncounters.Data
{
    public class EncounterData
    {
        public virtual OrderedCollection<Section> Sections { get; } = new OrderedCollection<Section>();
        public virtual VariableData Variables { get; }

        public EncounterData() {
            Variables = new VariableData();
        }

        public EncounterData(VariableData variables)
        {
            Variables = variables;
        }
    }
}