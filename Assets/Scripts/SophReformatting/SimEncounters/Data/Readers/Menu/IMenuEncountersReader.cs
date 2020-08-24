using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IMenuEncountersReader
    {
        WaitableResult<List<MenuEncounter>> GetMenuEncounters(User user);
    }
}