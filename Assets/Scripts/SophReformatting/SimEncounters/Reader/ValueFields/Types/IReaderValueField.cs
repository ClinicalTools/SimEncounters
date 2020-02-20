namespace ClinicalTools.SimEncounters.Reader
{
    public interface IReaderValueField
    {
        string Name { get; }
        string Value { get; }
        void Initialize(ReaderScene reader);
        void Initialize(ReaderScene reader, string value);
    }
}