using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IMetadataGroupsReader
    {
        WaitableResult<Dictionary<int, Dictionary<SaveType, EncounterMetadata>>> GetMetadataGroups(User user);
    }
}