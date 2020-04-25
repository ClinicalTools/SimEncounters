using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseChildPanelsDrawer : MonoBehaviour
    {
        public abstract List<BaseReaderPanelUI> DrawChildPanels(IEnumerable<UserPanel> childPanels);
    }

    public class ReaderPanel : BaseReaderPanelUI
    {
        [SerializeField] private BaseChildPanelsDrawer childPanelCreator;
        public BaseChildPanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }

        protected BasicReaderPanelDrawer BasicPanelDrawer { get; set; }
        [Inject]
        public void Inject(BasicReaderPanelDrawer basicPanelDrawer) => BasicPanelDrawer = basicPanelDrawer;

        public override void Display(UserPanel panel)
        {
            BasicPanelDrawer.Display(panel, transform, transform);
            if (ChildPanelCreator != null)
                ChildPanelCreator.DrawChildPanels(panel.GetChildPanels());
        }
    }
}