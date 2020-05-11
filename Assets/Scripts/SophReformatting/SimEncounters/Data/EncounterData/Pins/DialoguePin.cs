using ClinicalTools.SimEncounters.Collections;

namespace ClinicalTools.SimEncounters.Data
{
    public class DialoguePin
    {
        public virtual OrderedCollection<Panel> Conversation { get; set; } = new OrderedCollection<Panel>();
    }
}