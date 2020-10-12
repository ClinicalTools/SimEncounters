using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{

    public class ReaderPanel : BaseReaderPanel
    {
        public BaseReaderPanelsCreator ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }
        [SerializeField] private BaseReaderPanelsCreator childPanelCreator;

        protected UserPanel Panel { get; set; }

        protected IReaderPanelDisplay BasicPanelDrawer { get; set; }
        [Inject]
        public virtual void Inject(IReaderPanelDisplay basicPanelDrawer)
        {
            BasicPanelDrawer = basicPanelDrawer;
            if (Panel != null)
                ActualPanelDisplay();
        }



        public override void Display(UserPanel panel)
        {
            Panel = panel;
            if (BasicPanelDrawer != null)
                ActualPanelDisplay();
        }

        protected virtual void ActualPanelDisplay()
        {
            BasicPanelDrawer.Display(Panel, transform, transform);
            if (ChildPanelCreator != null)
                ChildPanelCreator.DrawChildPanels(Panel.GetChildPanels());
        }
    }
}