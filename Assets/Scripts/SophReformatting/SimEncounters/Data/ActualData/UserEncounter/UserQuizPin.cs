using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class UserQuizPin
    {
        public UserEncounter Encounter { get; }
        public QuizPin Data { get; }
        public UserQuizPin(UserEncounter encounter, QuizPin quiz)
        {
            Encounter = encounter;
            Data = quiz;
        }
        protected virtual Dictionary<string, UserPanel> Panels { get; } = new Dictionary<string, UserPanel>();
        public virtual List<UserPanel> GetPanels()
        {
            var userPanels = new List<UserPanel>();
            foreach (var panel in Data.Questions)
                userPanels.Add(GetPanel(panel.Key));

            return userPanels;
        }
        public virtual UserPanel GetPanel(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panel = Data.Questions[key];
            var userPanel = new UserPanel(Encounter, panel, null);
            Panels.Add(key, userPanel);
            return userPanel;
        }
    }
}