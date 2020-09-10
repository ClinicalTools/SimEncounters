using System;

namespace ClinicalTools.SimEncounters
{
    public interface ICompletionDrawer
    {
        void CompletionDraw(Encounter encounter);
    }
}