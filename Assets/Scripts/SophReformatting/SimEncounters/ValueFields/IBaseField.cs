namespace ClinicalTools.SimEncounters
{
    public interface IEncounterField : INamedField
    {
        void Initialize(Encounter encounter);
        void Initialize(Encounter encounter, string value);
    }
    public interface IValueField : INamedField
    {
        void Initialize();
        void Initialize(string value);
    }
    public interface INamedField
    {
        string Name { get; }
        string Value { get; }
    }
}