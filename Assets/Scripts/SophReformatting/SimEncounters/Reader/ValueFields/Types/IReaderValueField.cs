using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface IUserValueField
    {
        string Name { get; }
        string Value { get; }
        void Initialize(UserPanel userPanel);
        void Initialize(UserPanel userPanel, string value);
    }
}