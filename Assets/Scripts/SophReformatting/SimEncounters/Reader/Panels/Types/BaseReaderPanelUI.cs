using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderPanelUI : UserPanelDrawer, IReaderPanelUI
    {
        [SerializeField] private string type;
        public virtual string Type { get => type; set => type = value; }
    }
}