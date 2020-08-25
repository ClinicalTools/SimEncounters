using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseWriterPinsDrawer : MonoBehaviour
    {
        public abstract void Display(Encounter encounter, PinData pinData);

        public abstract PinData Serialize();
    }
}