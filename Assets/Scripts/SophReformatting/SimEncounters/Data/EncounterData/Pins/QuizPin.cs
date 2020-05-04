using ClinicalTools.SimEncounters.Collections;

namespace ClinicalTools.SimEncounters.Data
{
    public class QuizPin
    {
        public virtual OrderedCollection<Panel> Questions { get; } = new OrderedCollection<Panel>();
    }
}