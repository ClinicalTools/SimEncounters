using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserSectionDrawer : MonoBehaviour, IUserSectionDrawer
    {
        public abstract void Display(UserSectionSelectedEventArgs eventArgs);
    }
}