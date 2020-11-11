namespace ClinicalTools.SimEncounters
{
    public class ReaderEncounterManager :
        ISelector<UserEncounterSelectedEventArgs>,
        ISelector<UserSectionSelectedEventArgs>,
        ISelector<UserTabSelectedEventArgs>,
        ISelectedListener<EncounterSelectedEventArgs>,
        ISelectedListener<SectionSelectedEventArgs>,
        ISelectedListener<TabSelectedEventArgs>,
        ISelectedListener<EncounterMetadataSelectedEventArgs>
    {
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; } = new Selector<UserEncounterSelectedEventArgs>();
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; } = new Selector<UserSectionSelectedEventArgs>();
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; } = new Selector<UserTabSelectedEventArgs>();
        protected ISelector<EncounterSelectedEventArgs> EncounterSelector { get; } = new Selector<EncounterSelectedEventArgs>();
        protected ISelector<SectionSelectedEventArgs> SectionSelector { get; } = new Selector<SectionSelectedEventArgs>();
        protected ISelector<TabSelectedEventArgs> TabSelector { get; } = new Selector<TabSelectedEventArgs>();
        protected ISelector<EncounterMetadataSelectedEventArgs> MetadataSelector { get; } = new Selector<EncounterMetadataSelectedEventArgs>();

        public virtual void AddSelectedListener(SelectedHandler<UserEncounterSelectedEventArgs> handler) => UserEncounterSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<UserSectionSelectedEventArgs> handler) => UserSectionSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<UserTabSelectedEventArgs> handler) => UserTabSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<EncounterSelectedEventArgs> handler) => EncounterSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<SectionSelectedEventArgs> handler) => SectionSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<TabSelectedEventArgs> handler) => TabSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<EncounterMetadataSelectedEventArgs> handler) => MetadataSelector.AddSelectedListener(handler);

        public virtual void RemoveSelectedListener(SelectedHandler<UserEncounterSelectedEventArgs> handler) => UserEncounterSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<UserSectionSelectedEventArgs> handler) => UserSectionSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<UserTabSelectedEventArgs> handler) => UserTabSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<EncounterSelectedEventArgs> handler) => EncounterSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<SectionSelectedEventArgs> handler) => SectionSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<TabSelectedEventArgs> handler) => TabSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<EncounterMetadataSelectedEventArgs> handler) => MetadataSelector.RemoveSelectedListener(handler);

        UserEncounterSelectedEventArgs ISelectedListener<UserEncounterSelectedEventArgs>.CurrentValue => UserEncounterSelector.CurrentValue;
        UserSectionSelectedEventArgs ISelectedListener<UserSectionSelectedEventArgs>.CurrentValue => UserSectionSelector.CurrentValue;
        UserTabSelectedEventArgs ISelectedListener<UserTabSelectedEventArgs>.CurrentValue => UserTabSelector.CurrentValue;
        EncounterSelectedEventArgs ISelectedListener<EncounterSelectedEventArgs>.CurrentValue => EncounterSelector.CurrentValue;
        SectionSelectedEventArgs ISelectedListener<SectionSelectedEventArgs>.CurrentValue => SectionSelector.CurrentValue;
        TabSelectedEventArgs ISelectedListener<TabSelectedEventArgs>.CurrentValue => TabSelector.CurrentValue;
        EncounterMetadataSelectedEventArgs ISelectedListener<EncounterMetadataSelectedEventArgs>.CurrentValue => MetadataSelector.CurrentValue;

        protected UserEncounter CurrentEncounter { get; set; }
        protected UserSection CurrentSection { get; set; }
        protected UserTab CurrentTab { get; set; }


        public virtual void Select(object sender, UserEncounterSelectedEventArgs eventArgs)
        {
            CurrentEncounter = eventArgs.Encounter;
            UserEncounterSelector.Select(sender, eventArgs);
            EncounterSelector.Select(sender, new EncounterSelectedEventArgs(CurrentEncounter.Data));
            MetadataSelector.Select(sender, new EncounterMetadataSelectedEventArgs(CurrentEncounter.Data.Metadata));
            Select(sender, new UserSectionSelectedEventArgs(CurrentEncounter.GetCurrentSection(), ChangeType.JumpTo));
        }

        public virtual void Select(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            CurrentSection = eventArgs.SelectedSection;
            CurrentEncounter.Data.Content.NonImageContent.SetCurrentSection(CurrentSection.Data);
            UserSectionSelector.Select(sender, eventArgs);
            SectionSelector.Select(sender, new SectionSelectedEventArgs(CurrentSection.Data));
            Select(sender, new UserTabSelectedEventArgs(CurrentSection.GetCurrentTab(), eventArgs.ChangeType));
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