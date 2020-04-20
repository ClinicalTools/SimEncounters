using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using ClinicalTools.SimEncounters.Collections;
using System.Collections.Generic;
using System;

namespace ClinicalTools.SimEncounters.Reader
{
    public class TabSelectedEventArgs : EventArgs
    {
        public UserTab SelectedTab { get; }
        public TabSelectedEventArgs(UserTab selectedTab)
        {
            SelectedTab = selectedTab;
        }
    }
    public delegate void TabSelectedHandler(object sender, TabSelectedEventArgs e);
    public interface ITabSelector
    {
        event TabSelectedHandler TabSelected;
        void SelectTab(UserTab userTab);
    }
    public abstract class TabSelector : MonoBehaviour, ITabSelector
    {
        public abstract void Display(UserSection section);
        public abstract event TabSelectedHandler TabSelected;
        public abstract void SelectTab(UserTab userTab);
    }
}