using ClinicalTools.SimEncounters.Collections;
using System;

namespace ClinicalTools.SimEncounters
{
    public class UserPanelSelectedEventArgs : EventArgs
    {
        public UserPanel SelectedPanel { get; }
        public bool Active { get; }
        public UserPanelSelectedEventArgs(UserPanel selectedPanel, bool active)
        {
            SelectedPanel = selectedPanel;
            Active = active;
        }
    }
    public class ChildPanelsSelectedEventArgs : EventArgs
    {
        public OrderedCollection<UserPanel> ChildPanels { get; }
        public bool Active { get; }
        public ChildPanelsSelectedEventArgs(OrderedCollection<UserPanel> childPanels, bool active)
        {
            ChildPanels = childPanels;
            Active = active;
        }
    }
}