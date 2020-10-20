namespace ClinicalTools.SimEncounters
{
    public class ReaderEncounterManager :
        ISelector<UserEncounterSelectedEventArgs>,
        ISelector<UserSectionSelectedEventArgs>,
        ISelector<UserTabSelectedEventArgs>,
        ISelectedListener<Encounter>,
        ISelectedListener<SectionSelectedEventArgs>,
        ISelectedListener<TabSelectedEventArgs>,
        ISelectedListener<EncounterMetadata>
    {
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; } = new Selector<UserEncounterSelectedEventArgs>();
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; } = new Selector<UserSectionSelectedEventArgs>();
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; } = new Selector<UserTabSelectedEventArgs>();
        protected ISelector<Encounter> EncounterSelector { get; } = new Selector<Encounter>();
        protected ISelector<SectionSelectedEventArgs> SectionSelector { get; } = new Selector<SectionSelectedEventArgs>();
        protected ISelector<TabSelectedEventArgs> TabSelector { get; } = new Selector<TabSelectedEventArgs>();
        protected ISelector<EncounterMetadata> MetadataSelector { get; } = new Selector<EncounterMetadata>();

        public virtual void AddSelectedListener(SelectedHandler<UserEncounterSelectedEventArgs> handler) => UserEncounterSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<UserSectionSelectedEventArgs> handler) => UserSectionSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<UserTabSelectedEventArgs> handler) => UserTabSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<Encounter> handler) => EncounterSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<SectionSelectedEventArgs> handler) => SectionSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<TabSelectedEventArgs> handler) => TabSelector.AddSelectedListener(handler);
        public virtual void AddSelectedListener(SelectedHandler<EncounterMetadata> handler) => MetadataSelector.AddSelectedListener(handler);

        public virtual void RemoveSelectedListener(SelectedHandler<UserEncounterSelectedEventArgs> handler) => UserEncounterSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<UserSectionSelectedEventArgs> handler) => UserSectionSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<UserTabSelectedEventArgs> handler) => UserTabSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<Encounter> handler) => EncounterSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<SectionSelectedEventArgs> handler) => SectionSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<TabSelectedEventArgs> handler) => TabSelector.RemoveSelectedListener(handler);
        public virtual void RemoveSelectedListener(SelectedHandler<EncounterMetadata> handler) => MetadataSelector.RemoveSelectedListener(handler);

        protected UserEncounter CurrentEncounter { get; set; }
        protected UserSection CurrentSection { get; set; }
        protected UserTab CurrentTab { get; set; }
        public virtual void Select(object sender, UserEncounterSelectedEventArgs eventArgs)
        {
            CurrentEncounter = eventArgs.Encounter;
            UserEncounterSelector.Select(sender, eventArgs);
            EncounterSelector.Select(sender, CurrentEncounter.Data);
            MetadataSelector.Select(sender, CurrentEncounter.Data.Metadata);
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