using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabUI : MonoBehaviour, IReaderTabUI
    {
        public GameObject GameObject => gameObject;
        
        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private List<BaseReaderPanelUI> panelOptions;
        public virtual List<BaseReaderPanelUI> PanelOptions { get => panelOptions; set => panelOptions = value; }
    }
}