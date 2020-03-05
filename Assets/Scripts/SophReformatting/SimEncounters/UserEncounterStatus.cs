using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class UserEncounterStatus
    {
        public List<string> ReadPanels = new List<string>();
        public bool Completed { get; }
        public List<QuizAnswer> QuizAnswers { get; } = new List<QuizAnswer>();

        public UserEncounterStatus() { }
    }
}