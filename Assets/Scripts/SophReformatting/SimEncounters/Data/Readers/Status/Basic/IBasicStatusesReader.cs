using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IBasicStatusesReader
    {
        WaitableResult<Dictionary<int, EncounterBasicStatus>> GetBasicStatuses(User user);
    }
}