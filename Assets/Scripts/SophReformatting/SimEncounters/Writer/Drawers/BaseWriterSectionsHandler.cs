using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseWriterSectionsHandler : MonoBehaviour, ISectionSelector, IRearrangable
    {
        public abstract void Display(Encounter encounter);
        public abstract event SectionSelectedHandler SectionSelected;
        public abstract void SelectSection(Section section);
        public abstract event RearrangedHandler Rearranged;
    }
}