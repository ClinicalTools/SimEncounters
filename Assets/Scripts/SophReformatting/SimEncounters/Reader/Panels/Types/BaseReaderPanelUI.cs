using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderPanelUI : UserPanelDrawer, IReaderPanelUI
    {
        public abstract string Type { get; set; }
    }
}