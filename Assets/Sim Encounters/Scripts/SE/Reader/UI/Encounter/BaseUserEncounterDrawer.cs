using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserEncounterDrawer : MonoBehaviour, IUserEncounterDrawer
    {
        public abstract void Display(UserEncounter userEncounter);
    }
}