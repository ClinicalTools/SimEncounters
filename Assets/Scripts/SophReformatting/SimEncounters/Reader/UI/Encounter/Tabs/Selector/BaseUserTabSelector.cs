using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserTabSelector : MonoBehaviour, IUserTabSelector, IUserSectionDrawer
    {
        public abstract void Display(UserSectionSelectedEventArgs eventArgs);
        public abstract event UserTabSelectedHandler TabSelected;
        public abstract void Display(UserTabSelectedEventArgs eventArgs);
    }
}