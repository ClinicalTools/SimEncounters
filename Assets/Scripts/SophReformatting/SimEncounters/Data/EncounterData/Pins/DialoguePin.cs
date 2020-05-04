using ClinicalTools.SimEncounters.Collections;

namespace ClinicalTools.SimEncounters.Data
{
    public class DialoguePin
    {
        public virtual OrderedCollection<Panel> Conversation { get; } = new OrderedCollection<Panel>();
    }
}