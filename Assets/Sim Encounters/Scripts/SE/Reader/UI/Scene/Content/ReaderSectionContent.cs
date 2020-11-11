using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSectionContent : UserSectionSelectorBehaviour
    {
        public RectTransform RectTransform => (RectTransform)transform;
        public UserSection Section => CurrentSection;
        public UserTab Tab => CurrentTab;

        public void SetFirstTab(object sender, ChangeType changeType) 
            => Select(sender, new UserTabSelectedEventArgs(Section.Tabs[0].Value, changeType));
        public void SetCurrentTab(object sender, ChangeType changeType)
            => Select(sender, new UserTabSelectedEventArgs(Section.GetCurrentTab(), changeType));
        public void SetLastTab(object sender, ChangeType changeType)
            => Select(sender, new UserTabSelectedEventArgs(Section.Tabs[Section.Tabs.Count - 1].Value, changeType));
    }
}
