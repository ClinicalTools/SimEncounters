using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface IReaderTabUI
    {
        Transform PanelsParent { get; set; }

        void Initialize(EncounterReader reader, string tabFolder, Tab tab);
        void Serialize();
        void Destroy();
    }
}