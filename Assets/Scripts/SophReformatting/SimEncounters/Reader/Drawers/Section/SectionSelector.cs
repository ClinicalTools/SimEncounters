using ClinicalTools.SimEncounters.Collections;

using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class UserSectionSelectedEventArgs : EventArgs
    {
        public UserSection SelectedSection { get; }
        public UserSectionSelectedEventArgs(UserSection selectedSection)
        {
            SelectedSection = selectedSection;
        }
    }
    public delegate void SectionSelectedHandler(object sender, UserSectionSelectedEventArgs e);
}