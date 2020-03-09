using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class UserEncounterStatus
    {
        public List<string> ReadPanels = new List<string>();
        public bool Completed { get; set; }
        public List<QuizAnswer> QuizAnswers { get; } = new List<QuizAnswer>();
        public List<string> ReadTabs { get; } = new List<string>();
        public int Rating { get; set; }
        public long Timestamp { get; set; }

        public UserEncounterStatus() { }
    }
}