using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectorBehaviour : MonoBehaviour,
        ISelector<UserSectionSelectedEventArgs>,
        ISelector<UserTabSelectedEventArgs>,
        ISelectedListener<SectionSelectedEventArgs>,
        ISelectedListener<TabSelectedEventArgs>
    {
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; } = new Selector<UserSectionSelectedEventArgs>();
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; } = new Selector<UserTabSelectedEventArgs>();
        protected ISelector<SectionSelectedEventArgs> SectionSelector { get; } = new Selector<SectionSelectedEventArgs>();
        protected ISelector<TabSelectedEventArgs> TabSelector { get; } = new Selector<TabSelectedEventArgs>();

        public virtual void AddSelectedListener(SelectedHandler<UserSectionSelectedEventArgs> handler) => UserSectionSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<UserTabSelectedEventArgs> handler) => UserTabSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<SectionSelectedEventArgs> handler) => SectionSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<TabSelectedEventArgs> handler) => TabSelector.AddSelectedListener(handler);

        public virtual void RemoveSelectedListener(SelectedHandler<UserSectionSelectedEventArgs> handler) => UserSectionSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<UserTabSelectedEventArgs> handler) => UserTabSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<SectionSelectedEventArgs> handler) => SectionSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<TabSelectedEventArgs> handler) => TabSelector.RemoveSelectedListener(handler);

        protected UserSection CurrentSection { get; set; }
        protected UserTab CurrentTab { get; set; }

        UserSectionSelectedEventArgs ISelectedListener<UserSectionSelectedEventArgs>.CurrentValue => UserSectionSelector.CurrentValue;
        UserTabSelectedEventArgs ISelectedListener<UserTabSelectedEventArgs>.CurrentValue => UserTabSelector.CurrentValue;
        SectionSelectedEventArgs ISelectedListener<SectionSelectedEventArgs>.CurrentValue => SectionSelector.CurrentValue;
        TabSelectedEventArgs ISelectedListener<TabSelectedEventArgs>.CurrentValue => TabSelector.CurrentValue;

        public virtual void Select(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            CurrentSection = eventArgs.SelectedSection;
            UserSectionSelector.Select(sender, eventArgs);
            SectionSelector.Select(sender, new SectionSelectedEventArgs(CurrentSection.Data));
        }

        public virtual void Select(object sender, UserTabSelectedEventArgs eventArgs)
        {
            CurrentTab = eventArgs.SelectedTab;
            CurrentSection.Data.SetCurrentTab(CurrentTab.Data);
            UserTabSelector.Select(sender, eventArgs);
            TabSelector.Select(sender, new TabSelectedEventArgs(CurrentTab.Data));
        }
    }
}