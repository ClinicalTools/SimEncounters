using ClinicalTools.SimEncounters.Data;
using System;

namespace ClinicalTools.SimEncounters.Reader
{
    public class UserTabSelectedEventArgs : EventArgs
    {
        public UserTab SelectedTab { get; }
        public UserTabSelectedEventArgs(UserTab selectedTab)
        {
            SelectedTab = selectedTab;
        }
    }
    public delegate void UserTabSelectedHandler(object sender, UserTabSelectedEventArgs e);
    public interface IUserTabSelector
    {
        event UserTabSelectedHandler TabSelected;
        void SelectTab(UserTab userTab);
    }
}