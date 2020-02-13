using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public enum Difficulty
    {
        None, Beginner, Intermediate, Advanced
    }
    public class EncounterInfo
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public List<string> Categories { get; } = new List<string>();
        public string Audience { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}