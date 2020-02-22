using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTreatmentTabUI : MonoBehaviour, IReaderTabUI
    {
        public GameObject GameObject => gameObject;

        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private ReaderPanelUI treatmentPanel;
        public virtual ReaderPanelUI TreatmentPanel { get => treatmentPanel; set => treatmentPanel = value; }
    }
}