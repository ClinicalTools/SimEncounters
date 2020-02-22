using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderPanelUI : MonoBehaviour, IReaderPanelUI
    {
        public abstract string Type { get; set; }

        public KeyValuePair<string, Panel> KeyedPanel { get; private set; }

        public virtual void Initialize(ReaderScene reader, KeyValuePair<string, Panel> keyedPanel) {
            KeyedPanel = keyedPanel;
        }
    }

    public interface IReaderPanelUI
    {
        string Type { get; }
        KeyValuePair<string, Panel> KeyedPanel { get; }
    }
}