using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class EncounterDetailedStatus
    {
        public HashSet<string> ReadTabs { get; } = new HashSet<string>();
        public List<QuizAnswer> QuizAnswers { get; } = new List<QuizAnswer>();
        public long Timestamp { get; set; }
    }
}