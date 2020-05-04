using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseSectionSelector : MonoBehaviour, ISectionSelector
    {
        public abstract void Display(Encounter encounter);
        public abstract event SectionSelectedHandler SectionSelected;
        public abstract void SelectSection(Section section);
    }
}