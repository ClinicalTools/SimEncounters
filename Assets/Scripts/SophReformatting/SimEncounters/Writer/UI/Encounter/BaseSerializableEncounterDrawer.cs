using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseSerializableEncounterDrawer : MonoBehaviour
    {
        public abstract void Display(Encounter encounter);

        public abstract void Serialize();
    }
}