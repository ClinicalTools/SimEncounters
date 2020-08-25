﻿using System;

namespace ClinicalTools.SimEncounters
{
    public delegate void UserTabSelectedHandler(object sender, UserTabSelectedEventArgs e);
    public class UserTabSelectedEventArgs : EventArgs
    {
        public UserTab SelectedTab { get; }
        public UserTabSelectedEventArgs(UserTab selectedTab)
        {
            SelectedTab = selectedTab;
        }
    }
}