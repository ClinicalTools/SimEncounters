﻿using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ToggleValueField : BaseValueField
    {
        public override string Name => name;
        public override string Value => Toggle.isOn ? true.ToString() : null;

        private Toggle toggle;
        protected Toggle Toggle {
            get {
                if (toggle == null)
                    toggle = GetComponent<Toggle>();
                return toggle;
            }
        }

        public override void Initialize() { }
        public override void Initialize(string value)
        {
            if (bool.TryParse(value, out var boolVal))
                Toggle.isOn = boolVal;
        }
    }
}