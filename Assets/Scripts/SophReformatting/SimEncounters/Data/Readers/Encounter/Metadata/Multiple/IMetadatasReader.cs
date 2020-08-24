using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IMetadatasReader
    {
        WaitableResult<List<EncounterMetadata>> GetMetadatas(User user);
    }
}
