using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseUserSectionSelector : MonoBehaviour, IUserSectionSelector
    {
        public abstract void Display(UserEncounter encounter);
        public abstract event SectionSelectedHandler SectionSelected;
        public abstract void SelectSection(UserSection section);
    }
}