using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseUserEncounterDrawer : MonoBehaviour
    {
        public abstract void Display(UserEncounter userEncounter);
    }
}