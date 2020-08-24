
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseTabDrawer : MonoBehaviour
    {
        public abstract void Display(Encounter encounter, Tab tab);
        public abstract Tab Serialize();
    }
}