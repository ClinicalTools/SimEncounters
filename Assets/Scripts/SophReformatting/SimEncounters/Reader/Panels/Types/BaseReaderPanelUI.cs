using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderPanelUI : MonoBehaviour, IReaderPanelUI
    {
        public abstract string Type { get; set; }
    }
}