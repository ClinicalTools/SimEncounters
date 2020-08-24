
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IDetailedStatusWriter
    {
        void WriteStatus(UserEncounter encounter);
    }
}