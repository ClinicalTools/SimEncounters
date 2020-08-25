
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseUserEncounterDrawer : MonoBehaviour
    {
        public abstract void Display(UserEncounter userEncounter);
    }
}