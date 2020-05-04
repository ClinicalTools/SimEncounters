using ClinicalTools.SimEncounters.Collections;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public class UserTab
    {
        public UserEncounter Encounter { get; }
        public Tab Data { get; }
        protected TabStatus Status { get; }
        public event Action StatusChanged;
        public UserTab(UserEncounter encounter, Tab data, TabStatus status)
        {
            Encounter = encounter;
            Data = data;
            Status = status;

            foreach (var panel in data.Panels) {
                var userPanel = new UserPanel(encounter, panel.Value, status.GetPanelStatus(panel.Key));
                Panels.Add(panel.Key, userPanel);
            }
        }

        public bool IsRead() => Status.Read;
        public void SetRead(bool read)
        {
            if (Status.Read == read)
                return;
            Status.Read = read;
            StatusChanged?.Invoke();
        }

        protected virtual OrderedCollection<UserPanel> Panels { get; } = new OrderedCollection<UserPanel>();
        public virtual IEnumerable<UserPanel> GetPanels() => Panels.Values;
        public virtual UserPanel GetPanel(string key) => Panels[key];
    }
}