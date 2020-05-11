using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IField
    {
        string Name { get; }
        string Value { get; }
    }
    public interface IValueField : IField
    {
        void Initialize();
        void Initialize(string value);
    }
    public interface IEncounterField : IField
    {
        void Initialize(Encounter encounter);
        void Initialize(Encounter encounter, string value);
    }

}