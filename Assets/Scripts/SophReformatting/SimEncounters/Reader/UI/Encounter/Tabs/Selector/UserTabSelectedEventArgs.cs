using System;

namespace ClinicalTools.SimEncounters
{
    public enum ChangeType
    {
        Inactive, Previous, Next, JumpTo
    }

    public delegate void UserTabSelectedHandler(object sender, UserTabSelectedEventArgs e);
    public class UserTabSelectedEventArgs : EventArgs
    {
        public UserTab SelectedTab { get; }
        public ChangeType ChangeType { get; }
        public UserTabSelectedEventArgs(UserTab selectedTab, ChangeType changeType)
        {
            SelectedTab = selectedTab;
            ChangeType = changeType;
        }
    }
}