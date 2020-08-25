using System;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderDialogueChoice : BaseReaderPanel
    {
        public abstract event Action Completed;
    }
}