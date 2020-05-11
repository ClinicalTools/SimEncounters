using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public interface IWriterPanelValueDisplay
    {
        IField[] Display(Encounter encounter, Panel panel, Transform panelTransform);
    }
}