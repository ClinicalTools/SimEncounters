﻿using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    [Serializable]
    public class LabeledPanelUI
    {
        [SerializeField] private string label;
        public string Label { get => label; set => label = value; }

        [SerializeField] private WriterPanelUI panelUI;
        public WriterPanelUI PanelUI { get => panelUI; set => panelUI = value; }
    }
}