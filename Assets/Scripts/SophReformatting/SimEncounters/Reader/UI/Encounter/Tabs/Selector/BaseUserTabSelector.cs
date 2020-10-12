using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserTabSelector : MonoBehaviour, IUserTabSelector, IUserSectionDrawer
    {
        public abstract void Display(UserSection section);
        public abstract event UserTabSelectedHandler TabSelected;
        public abstract void Display(UserTab userTab);
    }
}