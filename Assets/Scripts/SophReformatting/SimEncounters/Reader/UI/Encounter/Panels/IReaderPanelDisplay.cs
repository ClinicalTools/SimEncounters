
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface IReaderPanelDisplay
    {
        void Display(UserPanel userPanel, Transform panelTransform, Transform pinParent);
    }
}