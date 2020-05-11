using ClinicalTools.SimEncounters.Data;
using System;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabSelectedEventArgs : EventArgs
    {
        public Tab SelectedTab { get; }
        public TabSelectedEventArgs(Tab selectedTab)
        {
            SelectedTab = selectedTab;
        }
    }
    public delegate void TabSelectedHandler(object sender, TabSelectedEventArgs e);
    public interface ITabSelector
    {
        event TabSelectedHandler TabSelected;
        void SelectTab(Tab tab);
    }
}