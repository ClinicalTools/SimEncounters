using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class ChildPanelsDrawer : MonoBehaviour
    {
        public abstract List<BaseReaderPanelUI> DrawChildPanels(IEnumerable<UserPanel> childPanels);
    }

    public class ReaderPanel : BaseReaderPanelUI
    {
        [SerializeField] private ChildPanelsDrawer childPanelCreator;
        public ChildPanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }

        protected BasicReaderPanelDrawer BasicPanelDrawer { get; set; }
        [Inject]
        public void Inject(BasicReaderPanelDrawer basicPanelDrawer) => BasicPanelDrawer = basicPanelDrawer;

        public override void Display(UserPanel userPanel)
        {
            BasicPanelDrawer.Display(userPanel, transform, transform);
            if (ChildPanelCreator != null)
                ChildPanelCreator.DrawChildPanels(userPanel.GetChildPanels());
        }
    }
}