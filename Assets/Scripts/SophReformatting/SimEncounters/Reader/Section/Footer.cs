using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class Footer : MonoBehaviour, ISectionSelector, ITabSelector
    {
        public abstract void Display(UserEncounter userEncounter);
        public abstract event SectionSelectedHandler SectionSelected;
        public abstract void SelectSection(UserSection userSection);
        public abstract event TabSelectedHandler TabSelected;
        public abstract void SelectTab(UserTab userTab);
    }
}