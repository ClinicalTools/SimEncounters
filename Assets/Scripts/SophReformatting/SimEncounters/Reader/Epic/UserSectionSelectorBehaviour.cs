namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelectorBehaviour : UserTabSelectorBehaviour,
        ISelector<UserSectionSelectedEventArgs>,
        ISelectedListener<SectionSelectedEventArgs>
    {
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; } = new Selector<UserSectionSelectedEventArgs>();
        protected ISelector<SectionSelectedEventArgs> SectionSelector { get; } = new Selector<SectionSelectedEventArgs>();

        UserSectionSelectedEventArgs ISelectedListener<UserSectionSelectedEventArgs>.CurrentValue => UserSectionSelector.CurrentValue;
        SectionSelectedEventArgs ISelectedListener<SectionSelectedEventArgs>.CurrentValue => SectionSelector.CurrentValue;

        protected UserSection CurrentSection { get; set; }

        public virtual void AddSelectedListener(SelectedHandler<UserSectionSelectedEventArgs> handler) => UserSectionSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<SectionSelectedEventArgs> handler) => SectionSelector.AddSelectedListener(handler);

        public virtual void RemoveSelectedListener(SelectedHandler<UserSectionSelectedEventArgs> handler) => UserSectionSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<SectionSelectedEventArgs> handler) => SectionSelector.RemoveSelectedListener(handler);

        public virtual void Select(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            CurrentSection = eventArgs.SelectedSection;
            UserSectionSelector.Select(sender, eventArgs);
            SectionSelector.Select(sender, new SectionSelectedEventArgs(CurrentSection.Data));
        }

        public override void Select(object sender, UserTabSelectedEventArgs eventArgs)
        {
            CurrentSection.Data.SetCurrentTab(eventArgs.SelectedTab.Data);
            base.Select(sender, eventArgs);
        }
    }
}