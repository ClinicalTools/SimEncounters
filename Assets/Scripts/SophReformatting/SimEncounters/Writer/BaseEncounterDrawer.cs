using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseEncounterDrawer : MonoBehaviour
    {
        public abstract void Display(Encounter encounter);
    }
}