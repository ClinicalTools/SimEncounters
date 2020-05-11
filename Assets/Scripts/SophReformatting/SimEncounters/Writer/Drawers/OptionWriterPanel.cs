using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    [Serializable]
    public class OptionWriterPanel
    {
        public string DisplayText { get => displayText; set => displayText = value; }
        [SerializeField] private string displayText;
        public BaseWriterPanel PanelPrefab { get => panelPrefab; set => panelPrefab = value; }
        [SerializeField] private BaseWriterPanel panelPrefab;

    }
}