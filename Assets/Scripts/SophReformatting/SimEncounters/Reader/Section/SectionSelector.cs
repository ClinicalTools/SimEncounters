using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class SectionSelectedEventArgs : EventArgs
    {
        public UserSection SelectedSection { get; }
        public SectionSelectedEventArgs(UserSection selectedSection)
        {
            SelectedSection = selectedSection;
        }
    }
    public delegate void SectionSelectedHandler(object sender, SectionSelectedEventArgs e);
    public interface ISectionSelector
    {
        event SectionSelectedHandler SectionSelected;
        void SelectSection(UserSection userSection);
    }
    public abstract class SectionSelector : MonoBehaviour, ISectionSelector
    {
        public abstract void Display(UserEncounter userEncounter);
        public abstract event SectionSelectedHandler SectionSelected;
        public abstract void SelectSection(UserSection userSection);
    }
}