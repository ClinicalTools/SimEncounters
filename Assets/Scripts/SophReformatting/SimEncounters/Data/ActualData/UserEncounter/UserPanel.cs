using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class UserPanel
    {
        public UserEncounter Encounter { get; }
        public Panel Data { get; }
        protected PanelStatus Status { get; }
        public UserPinGroup PinGroup { get; }

        public UserPanel(UserEncounter encounter, Panel panel, PanelStatus status)
        {
            Encounter = encounter;
            Data = panel;
            Status = status;
            if (panel.Pins != null && panel.Pins.HasPin())
                PinGroup = new UserPinGroup(encounter, panel.Pins);
        }

        protected virtual Dictionary<string, UserPanel> Panels { get; } = new Dictionary<string, UserPanel>();
        public virtual List<UserPanel> GetChildPanels()
        {
            var userPanels = new List<UserPanel>();
            foreach (var panel in Data.ChildPanels)
                userPanels.Add(GetPanel(panel.Key));

            return userPanels;
        }
        public virtual UserPanel GetPanel(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panel = Data.ChildPanels[key];
            //var panelStatus = Status.GetChildPanelStatus(key);
            var userPanel = new UserPanel(Encounter, panel, null);
            Panels.Add(key, userPanel);
            return userPanel;
        }
    }
}