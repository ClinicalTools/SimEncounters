using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserTabDrawer : MonoBehaviour, IUserTabDrawer
    {
        public abstract void Display(UserTab tab);
    }
}