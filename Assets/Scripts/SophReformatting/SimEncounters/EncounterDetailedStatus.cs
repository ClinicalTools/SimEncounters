using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class EncounterDetailedStatus
    {
        public List<string> ReadPanels { get; } = new List<string>();
        public List<QuizAnswer> QuizAnswers { get; } = new List<QuizAnswer>();
        public List<string> ReadTabs { get; } = new List<string>();
        public long Timestamp { get; set; }
    }
}