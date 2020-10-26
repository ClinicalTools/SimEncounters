using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class UserPanelSelectorBehaviour : MonoBehaviour,
        ISelector<UserPanelSelectedEventArgs>,
        ISelectedListener<Panel>
    {
        protected ISelector<UserPanelSelectedEventArgs> UserPanelSelector { get; } = new Selector<UserPanelSelectedEventArgs>();
        protected ISelector<Panel> PanelSelector { get; } = new Selector<Panel>();

        UserPanelSelectedEventArgs ISelectedListener<UserPanelSelectedEventArgs>.CurrentValue => UserPanelSelector.CurrentValue;
        Panel ISelectedListener<Panel>.CurrentValue => PanelSelector.CurrentValue;

        public virtual void AddSelectedListener(SelectedHandler<UserPanelSelectedEventArgs> handler)
            => UserPanelSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<Panel> handler) => PanelSelector.AddSelectedListener(handler);

        public virtual void RemoveSelectedListener(SelectedHandler<UserPanelSelectedEventArgs> handler) 
            => UserPanelSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<Panel> handler) => PanelSelector.RemoveSelectedListener(handler);

        public virtual void Select(object sender, UserPanelSelectedEventArgs userPanel)
        {
            UserPanelSelector.Select(sender, userPanel);
            PanelSelector.Select(sender, userPanel.SelectedPanel.Data);
        }
    }
}