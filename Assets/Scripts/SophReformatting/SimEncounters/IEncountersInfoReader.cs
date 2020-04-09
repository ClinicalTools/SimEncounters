using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IDetailedStatusWriter
    {
        void DoStuff(User user, FullEncounter encounter);
    }
}