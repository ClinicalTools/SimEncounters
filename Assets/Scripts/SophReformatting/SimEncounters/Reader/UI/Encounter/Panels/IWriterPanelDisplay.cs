
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public interface IWriterPanelValueDisplay
    {
        BaseField[] Display(Encounter encounter, Panel panel, Transform panelTransform);
    }
}