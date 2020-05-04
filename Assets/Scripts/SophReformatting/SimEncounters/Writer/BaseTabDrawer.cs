using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseTabDrawer : MonoBehaviour
    {
        public abstract void Display(Encounter encounter, Tab tab);
    }
}