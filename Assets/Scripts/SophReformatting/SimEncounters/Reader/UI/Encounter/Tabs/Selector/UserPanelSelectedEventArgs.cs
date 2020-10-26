using System;

namespace ClinicalTools.SimEncounters
{
    public class UserPanelSelectedEventArgs : EventArgs
    {
        public UserPanel SelectedPanel { get; }
        public ChangeType ChangeType { get; }
        public UserPanelSelectedEventArgs(UserPanel selectedPanel, ChangeType changeType)
        {
            SelectedPanel = selectedPanel;
            ChangeType = changeType;
        }
    }
}