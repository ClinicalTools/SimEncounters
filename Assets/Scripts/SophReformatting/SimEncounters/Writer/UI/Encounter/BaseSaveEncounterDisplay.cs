using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseSaveEncounterDisplay : MonoBehaviour
    {
        public abstract void Display(User user, Encounter encounter);
    }
}