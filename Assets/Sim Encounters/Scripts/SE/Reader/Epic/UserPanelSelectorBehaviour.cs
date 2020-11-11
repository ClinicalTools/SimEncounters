using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class UserPanelSelectorBehaviour : MonoBehaviour,
        ISelector<UserPanelSelectedEventArgs>,
        ISelectedListener<PanelSelectedEventArgs>
    {
        protected ISelector<UserPanelSelectedEventArgs> UserPanelSelector { get; } = new Selector<UserPanelSelectedEventArgs>();
        protected ISelector<PanelSelectedEventArgs> PanelSelector { get; } = new Selector<PanelSelectedEventArgs>();

        UserPanelSelectedEventArgs ISelectedListener<UserPanelSelectedEventArgs>.CurrentValue => UserPanelSelector.CurrentValue;
        PanelSelectedEventArgs ISelectedListener<PanelSelectedEventArgs>.CurrentValue => PanelSelector.CurrentValue;

        public virtual void AddSelectedListener(SelectedHandler<UserPanelSelectedEventArgs> handler)
            => UserPanelSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<PanelSelectedEventArgs> handler) => PanelSelector.AddSelectedListener(handler);

        public virtual void RemoveSelectedListener(SelectedHandler<UserPanelSelectedEventArgs> handler) 
            => UserPanelSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<PanelSelectedEventArgs> handler) => PanelSelector.RemoveSelectedListener(handler);

        public virtual void Select(object sender, UserPanelSelectedEventArgs userPanel)
        {
            UserPanelSelector.Select(sender, userPanel);
            PanelSelector.Select(sender, new PanelSelectedEventArgs(userPanel.SelectedPanel.Data));
        }
    }
}