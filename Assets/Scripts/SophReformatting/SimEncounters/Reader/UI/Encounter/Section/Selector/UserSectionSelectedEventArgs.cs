using System;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectedEventArgs : EventArgs
    {
        public UserSection SelectedSection { get; }
        public UserSectionSelectedEventArgs(UserSection selectedSection)
        {
            SelectedSection = selectedSection;
        }
    }
    public delegate void UserSectionSelectedHandler(object sender, UserSectionSelectedEventArgs e);
}