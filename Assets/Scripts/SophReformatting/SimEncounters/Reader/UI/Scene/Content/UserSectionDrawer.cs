using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionDrawer : UserTabDrawer
    {
        public ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        [Inject] public virtual void Inject(ISelector<UserSectionSelectedEventArgs> userSectionSelector) => UserSectionSelector = userSectionSelector;

        public UserSection Section { get; protected set; }
        public virtual void ChangeSection(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            Section = eventArgs.SelectedSection;
            UserSectionSelector.Select(sender, eventArgs);
        }
        public override void ChangeTab(object sender, UserTabSelectedEventArgs eventArgs)
        {
            Tab = eventArgs.SelectedTab;
            UserTabSelector.Select(sender, eventArgs);
        }

        public void SetFirstTab(object sender, ChangeType changeType) 
            => ChangeTab(sender, new UserTabSelectedEventArgs(Section.Tabs[0].Value, changeType));
        public void SetCurrentTab(object sender, ChangeType changeType)
            => ChangeTab(sender, new UserTabSelectedEventArgs(Section.GetCurrentTab(), changeType));
        public void SetLastTab(object sender, ChangeType changeType)
            => ChangeTab(sender, new UserTabSelectedEventArgs(Section.Tabs[Section.Tabs.Count - 1].Value, changeType));
    }
}
