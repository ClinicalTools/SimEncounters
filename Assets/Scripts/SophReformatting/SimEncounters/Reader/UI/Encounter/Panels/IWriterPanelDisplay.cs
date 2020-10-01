
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IWriterPanelValueDisplay
    {
        IPanelField[] Display(Encounter encounter, Panel panel, Transform panelTransform);
    }
}