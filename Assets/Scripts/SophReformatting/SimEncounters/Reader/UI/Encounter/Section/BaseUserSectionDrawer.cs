using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserSectionDrawer : MonoBehaviour
    {
        public abstract void Display(UserSection userSection);
    }
}