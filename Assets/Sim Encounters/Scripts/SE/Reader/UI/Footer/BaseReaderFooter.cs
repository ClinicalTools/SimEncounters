using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseReaderFooter : MonoBehaviour, IUserSectionSelector, IUserTabSelector, IUserEncounterDrawer, ICompletable
    {
        public abstract void Display(UserEncounter userEncounter);
        public abstract event UserSectionSelectedHandler SectionSelected;
        public abstract void Display(UserSectionSelectedEventArgs eventArgs);
        public abstract event UserTabSelectedHandler TabSelected;
        public abstract void Select(UserTabSelectedEventArgs eventArgs);

        public abstract event Action Completed;
    }
}