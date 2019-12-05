namespace ClinicalTools.SimEncounters.Data
{
    public class ConditionalData
    {
        public virtual KeyedCollection<BoolConditional> Bools { get; } = new KeyedCollection<BoolConditional>();
        public virtual KeyedCollection<IntConditional> Ints { get; } = new KeyedCollection<IntConditional>();

        public ConditionalData() { }
    }
}