using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserTabSelectorBehaviour : SelectorBehaviour<UserTabSelectedEventArgs>
    {
        protected ISelector<TabSelectedEventArgs> TabSelector { get; set; }
        [Inject] public virtual void Inject(ISelector<TabSelectedEventArgs> tabSelector) 
            => TabSelector = tabSelector;
        public override void Select(object sender, UserTabSelectedEventArgs value)
        {
            base.Select(sender, value);
            TabSelector.Select(this, new TabSelectedEventArgs(value.SelectedTab.Data));
        }
    }
}