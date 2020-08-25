using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseUserSectionDrawer : MonoBehaviour
    {
        public abstract void Display(UserSection userSection);
    }
}