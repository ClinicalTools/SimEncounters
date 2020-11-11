using System;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseReaderDialogueChoice : BaseReaderPanel
    {
        public abstract event Action Completed;
    }
}