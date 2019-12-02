namespace SimEncounters.Data
{
    public class NewEncounterData
    {
        public virtual OrderedDictionary<Section> Sections { get; } = new OrderedDictionary<Section>();
        public virtual VariableData Variables { get; }

        public NewEncounterData() {
            Variables = new VariableData();
        }

        public NewEncounterData(VariableData variables)
        {
            Variables = variables;
        }
    }
}