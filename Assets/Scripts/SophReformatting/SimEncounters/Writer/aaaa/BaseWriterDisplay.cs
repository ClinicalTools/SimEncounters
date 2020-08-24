using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseWriterDisplay : MonoBehaviour
    {
        public abstract void Display(User user, Encounter encounter);
    }
}