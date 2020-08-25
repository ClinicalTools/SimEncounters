using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseTabSelector : MonoBehaviour, ITabSelector
    {
        public abstract void Display(Encounter encounter, Section section);
        public abstract event TabSelectedHandler TabSelected;
        public abstract void SelectTab(Tab tab);
    }
}