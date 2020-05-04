using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{

    public class ReaderPanel : BaseReaderPanel
    {
        public BaseReaderPanelsCreator ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }
        [SerializeField] private BaseReaderPanelsCreator childPanelCreator;

        protected IReaderPanelDisplay BasicPanelDrawer { get; set; }
        [Inject] public virtual void Inject(IReaderPanelDisplay basicPanelDrawer) => BasicPanelDrawer = basicPanelDrawer;

        public override void Display(UserPanel panel)
        {
            BasicPanelDrawer.Display(panel, transform, transform);
            if (ChildPanelCreator != null)
                ChildPanelCreator.DrawChildPanels(panel.GetChildPanels());
        }
    }
}