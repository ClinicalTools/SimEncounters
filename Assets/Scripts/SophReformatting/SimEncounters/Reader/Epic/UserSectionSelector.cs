namespace ClinicalTools.SimEncounters
{
    public class UserSectionSelector : Selector<UserSectionSelectedEventArgs>
    {
        protected ISelector<SectionSelectedEventArgs> SectionSelector { get; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; }
        public UserSectionSelector(
            ISelector<UserTabSelectedEventArgs> userTabSelector,
            ISelector<SectionSelectedEventArgs> sectionSelector)
        {
            UserTabSelector = userTabSelector;
            UserTabSelector.AddEarlySelectedListener(OnTabSelected);
            SectionSelector = sectionSelector;
        }
        public override void Select(object sender, UserSectionSelectedEventArgs value)
        {
            base.Select(sender, value);
            var tabArgs = new UserTabSelectedEventArgs(value.SelectedSection.GetCurrentTab(), value.ChangeType);
            UserTabSelector.Select(this, tabArgs);
            SectionSelector.Select(this, new SectionSelectedEventArgs(value.SelectedSection.Data));
        }

        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
            => CurrentValue.SelectedSection.Data.SetCurrentTab(eventArgs.SelectedTab.Data);
    }
}