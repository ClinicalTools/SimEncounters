
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseSectionDrawer : MonoBehaviour
    {
        public abstract void Display(Encounter encounter, Section section);
    }
}