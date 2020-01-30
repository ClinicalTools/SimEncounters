using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public interface IWriterTabUI
    {
        Transform PanelsParent { get; set; }

        void Initialize(EncounterWriter writer, string tabFolder, Tab tab);
        void Serialize();
        void Destroy();
    }
}