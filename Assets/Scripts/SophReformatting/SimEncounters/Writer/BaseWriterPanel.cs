using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseWriterPanel : MonoBehaviour
    {
        public virtual string Type { get => type; set => type = value; }
        [SerializeField] private string type;

        public abstract void Display(Encounter encounter, Panel panel);
    }
}