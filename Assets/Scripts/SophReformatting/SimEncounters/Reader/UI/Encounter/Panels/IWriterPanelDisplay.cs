
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IWriterPanelValueDisplay
    {
        BaseField[] Display(Encounter encounter, Panel panel, Transform panelTransform);
    }
}