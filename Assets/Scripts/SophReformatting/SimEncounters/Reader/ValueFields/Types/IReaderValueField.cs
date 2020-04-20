using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface IReaderValueField
    {
        string Name { get; }
        string Value { get; }
        void Initialize(ReaderScene reader);
        void Initialize(ReaderScene reader, string value);
    }
    public interface IUserValueField
    {
        string Name { get; }
        string Value { get; }
        void Initialize(UserPanel userPanel);
        void Initialize(UserPanel userPanel, string value);
    }
}