
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public interface IReaderPanelDisplay
    {
        void Display(UserPanel userPanel, Transform panelTransform, Transform pinParent);
        void Dispose();
    }
}