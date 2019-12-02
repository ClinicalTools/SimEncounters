namespace SimEncounters.Data
{
    public class ConditionData
    {
        public virtual KeyedCollection<BoolConditional> Bools { get; } = new KeyedCollection<BoolConditional>();
        public virtual KeyedCollection<IntConditional> Ints { get; } = new KeyedCollection<IntConditional>();

        public ConditionData() { }
    }
}