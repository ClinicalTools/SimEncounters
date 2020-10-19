using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class UserSectionDrawer : UserTabDrawer
    {
        public UserSectionSelectorBehaviour UserSectionSelector { get => userSectionSelector; set => userSectionSelector = value; }
        [SerializeField] private UserSectionSelectorBehaviour userSectionSelector;

        public UserSection Section { get; protected set; }
        public virtual void ChangeSection(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            Section = eventArgs.SelectedSection;
            UserSectionSelector.Select(sender, eventArgs);
        }

        public void SetFirstTab(object sender, ChangeType changeType) 
            => ChangeTab(sender, new UserTabSelectedEventArgs(Section.Tabs[0].Value, changeType));
        public void SetCurrentTab(object sender, ChangeType changeType)
            => ChangeTab(sender, new UserTabSelectedEventArgs(Section.GetCurrentTab(), changeType));
        public void SetLastTab(object sender, ChangeType changeType)
            => ChangeTab(sender, new UserTabSelectedEventArgs(Section.Tabs[Section.Tabs.Count - 1].Value, changeType));
    }
}
