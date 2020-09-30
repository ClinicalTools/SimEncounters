
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IWriterPanelValueDisplay
    {
        INamedField[] Display(Encounter encounter, Panel panel, Transform panelTransform);
    }
}