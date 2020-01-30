namespace ClinicalTools.SimEncounters.Reader
{
    public interface IReaderValueField
    {
        string Name { get; }
        string Value { get; }
        void Initialize(EncounterReader reader);
        void Initialize(EncounterReader reader, string value);
    }
}