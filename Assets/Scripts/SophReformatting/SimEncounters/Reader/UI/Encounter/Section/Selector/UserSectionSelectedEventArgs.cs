using System;

namespace ClinicalTools.SimEncounters
{
    public delegate void UserSectionSelectedHandler(object sender, UserSectionSelectedEventArgs e);
    public class UserSectionSelectedEventArgs : EventArgs
    {
        public UserSection SelectedSection { get; }
        public ChangeType ChangeType { get; }
        public UserSectionSelectedEventArgs(UserSection selectedSection, ChangeType changeType)
        {
            SelectedSection = selectedSection;
            ChangeType = changeType;
        }
    }
}