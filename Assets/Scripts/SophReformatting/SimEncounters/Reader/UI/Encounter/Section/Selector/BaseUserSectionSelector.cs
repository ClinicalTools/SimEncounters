using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserSectionSelector : MonoBehaviour, IUserSectionSelector, IUserEncounterDrawer
    {
        public abstract void Display(UserEncounter encounter);
        public abstract event UserSectionSelectedHandler SectionSelected;
        public abstract void Display(UserSectionSelectedEventArgs eventArgs);
    }
}