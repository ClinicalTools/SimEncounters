namespace ClinicalTools.SimEncounters
{
    public interface IValueField
    {
        string Name { get; }
        string Value { get; }
        void Initialize();
        void Initialize(string value);
    }
}