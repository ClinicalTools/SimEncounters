using System;
using UnityEngine;
namespace ClinicalTools.SimEncounters.Reader
{
    [Serializable]
    public class KeyedPanelUI
    {
        [SerializeField] private string key;
        public string Key { get => key; set => key = value; }

        [SerializeField] private ReaderPanelUI panelUI;
        public ReaderPanelUI PanelUI { get => panelUI; set => panelUI = value; }
    }
}