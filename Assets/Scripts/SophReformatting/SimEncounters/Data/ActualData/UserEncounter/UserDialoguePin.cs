using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class UserDialoguePin
    {
        public UserEncounter Encounter { get; }
        public DialoguePin Data { get; }
        public UserDialoguePin(UserEncounter encounter, DialoguePin dialogue)
        {
            Encounter = encounter;
            Data = dialogue;
        }
        protected virtual Dictionary<string, UserPanel> Panels { get; } = new Dictionary<string, UserPanel>();
        public virtual List<UserPanel> GetPanels()
        {
            var userPanels = new List<UserPanel>();
            foreach (var panel in Data.Conversation)
                userPanels.Add(GetPanel(panel.Key));

            return userPanels;
        }
        public virtual UserPanel GetPanel(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panel = Data.Conversation[key];
            var userPanel = new UserPanel(Encounter, panel, null);
            Panels.Add(key, userPanel);
            return userPanel;
        }
    }
}