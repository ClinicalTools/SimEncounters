using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SectionContent : TabContent
    {
        public UserSection Section { get; protected set; }
        public virtual void ChangeSection(UserSectionSelectedEventArgs eventArgs)
        {
            Section = eventArgs.SelectedSection;
            if (Section != null && Behaviour is IUserSectionDrawer sectionDrawer)
                sectionDrawer.Display(eventArgs);
        }
        public SectionContent(MonoBehaviour behaviour) : base(behaviour) { }

        public void SetFirstTab(ChangeType changeType) 
            => ChangeTab(new UserTabSelectedEventArgs(Section.Tabs[0].Value, changeType));
        public void SetCurrentTab(ChangeType changeType)
            => ChangeTab(new UserTabSelectedEventArgs(Section.GetCurrentTab(), changeType));
        public void SetLastTab(ChangeType changeType)
            => ChangeTab(new UserTabSelectedEventArgs(Section.Tabs[Section.Tabs.Count - 1].Value, changeType));
    }
}
