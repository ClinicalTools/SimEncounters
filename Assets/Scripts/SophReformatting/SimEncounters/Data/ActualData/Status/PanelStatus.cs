using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class DialogueStatus
    {
        public bool Read { get; set; }
        protected Dictionary<string, PanelStatus> Panels { get; } = new Dictionary<string, PanelStatus>();
    }
    public class QuizStatus
    {
        public bool Read { get; set; }
        protected Dictionary<string, PanelStatus> Panels { get; } = new Dictionary<string, PanelStatus>();
    }

    public class PinDataStatus
    {
        public bool Read { get; set; }
        public QuizStatus QuizStatus { get; }
        public DialogueStatus DialogueStatus { get; }

    }

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