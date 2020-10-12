using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SectionContent : TabContent
    {
        private UserSection section;
        public UserSection Section {
            get => section;
            set {
                section = value;
                if (section != null && Behaviour is IUserSectionDrawer sectionDrawer)
                    sectionDrawer.Display(section);
            }
        }
        public SectionContent(MonoBehaviour behaviour) : base(behaviour) { }

        public void SetFirstTab() => Tab = section.Tabs[0].Value;
        public void SetCurrentTab() => Tab = section.GetCurrentTab();
        public void SetLastTab() => Tab = section.Tabs[section.Tabs.Count - 1].Value;
    }
}
