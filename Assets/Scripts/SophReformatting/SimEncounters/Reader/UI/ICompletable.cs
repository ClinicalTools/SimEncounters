using System;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface ICompletable
    {
        event Action Finish;
    }
}