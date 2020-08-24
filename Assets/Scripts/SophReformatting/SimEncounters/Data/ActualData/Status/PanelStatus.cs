using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public class PanelStatus
    {
        public bool Read { get; set; }

        protected Dictionary<string, PanelStatus> Panels { get; } = new Dictionary<string, PanelStatus>();

        public virtual PanelStatus GetChildPanelStatus(string key)
        {
            if (Panels.ContainsKey(key))
                return Panels[key];

            var panelStatus = new PanelStatus();
            Panels.Add(key, panelStatus);
            return panelStatus;
        }
    }
}