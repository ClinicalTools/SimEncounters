using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelUI : BaseReaderPanelUI
    {
        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private Transform childrenParent;
        public virtual Transform ChildrenParent { get => childrenParent; set => childrenParent = value; }

        [SerializeField] private List<ReaderPanelUI> childPanelOptions;
        public virtual List<ReaderPanelUI> ChildPanelOptions { get => childPanelOptions; set => childPanelOptions = value; }
    }
}