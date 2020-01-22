using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabUI : MonoBehaviour
    {
        [SerializeField] private List<LabeledPanelUI> panelOptions;
        public virtual List<LabeledPanelUI> PanelOptions { get => panelOptions; set => panelOptions = value; }

        [SerializeField] private Button addPanelButton;
        public virtual Button AddPanelButton { get => addPanelButton; set => addPanelButton = value; }


        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }
    }
}