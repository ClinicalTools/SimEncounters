using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderPanelUI : MonoBehaviour
    {
        public abstract string Type { get; set; }

        public KeyValuePair<string, Panel> KeyedPanel { get; private set; }

        public virtual void Initialize(EncounterReader reader, KeyValuePair<string, Panel> keyedPanel) {
            KeyedPanel = keyedPanel;
        }
    }
}