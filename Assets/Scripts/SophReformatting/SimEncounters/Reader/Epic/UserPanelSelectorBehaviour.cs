using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class UserPanelSelectorBehaviour : MonoBehaviour,
        ISelector<UserPanel>,
        ISelectedListener<Panel>
    {
        protected ISelector<UserPanel> UserPanelSelector { get; } = new Selector<UserPanel>();
        protected ISelector<Panel> PanelSelector { get; } = new Selector<Panel>();

        public virtual void AddSelectedListener(SelectedHandler<UserPanel> handler) => UserPanelSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<Panel> handler) => PanelSelector.AddSelectedListener(handler);

        public virtual void RemoveSelectedListener(SelectedHandler<UserPanel> handler) => UserPanelSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<Panel> handler) => PanelSelector.RemoveSelectedListener(handler);

        public virtual void Select(object sender, UserPanel userPanel)
        {
            UserPanelSelector.Select(sender, userPanel);
            PanelSelector.Select(sender, userPanel.Data);
        }
    }
}