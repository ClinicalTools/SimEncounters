using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class UserTabSelectorBehaviour : MonoBehaviour,
        ISelector<UserTabSelectedEventArgs>,
        ISelectedListener<TabSelectedEventArgs>
    {
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; } = new Selector<UserTabSelectedEventArgs>();
        protected ISelector<TabSelectedEventArgs> TabSelector { get; } = new Selector<TabSelectedEventArgs>();

        UserTabSelectedEventArgs ISelectedListener<UserTabSelectedEventArgs>.CurrentValue => UserTabSelector.CurrentValue;
        TabSelectedEventArgs ISelectedListener<TabSelectedEventArgs>.CurrentValue => TabSelector.CurrentValue;

        public virtual void AddSelectedListener(SelectedHandler<UserTabSelectedEventArgs> handler) => UserTabSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<TabSelectedEventArgs> handler) => TabSelector.AddSelectedListener(handler);

        public virtual void RemoveSelectedListener(SelectedHandler<UserTabSelectedEventArgs> handler) => UserTabSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<TabSelectedEventArgs> handler) => TabSelector.RemoveSelectedListener(handler);

        public virtual void Select(object sender, UserTabSelectedEventArgs eventArgs)
        {
            UserTabSelector.Select(sender, eventArgs);
            TabSelector.Select(sender, new TabSelectedEventArgs(eventArgs.SelectedTab.Data));
        }
    }
}