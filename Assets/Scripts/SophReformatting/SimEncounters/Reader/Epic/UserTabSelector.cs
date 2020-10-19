namespace ClinicalTools.SimEncounters
{
    public class UserTabSelector : Selector<UserTabSelectedEventArgs>
    {
        protected ISelector<Tab> TabSelector { get; }
        public UserTabSelector(ISelector<Tab> tabSelector)
            => TabSelector = tabSelector;

        public override void Select(object sender, UserTabSelectedEventArgs value)
        {
            base.Select(sender, value);
            TabSelector.Select(this, value.SelectedTab.Data);
        }
    }
}