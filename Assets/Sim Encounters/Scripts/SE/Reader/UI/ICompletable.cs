using System;

namespace ClinicalTools.SimEncounters
{
    public interface ICompletable
    {
        event Action Completed;
    }
}