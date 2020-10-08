using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderProgressBar : ReaderBehaviour, IUserTabDrawer, IUserEncounterDrawer
    {
        public RectTransform SectionDotPrefab { get => sectionDotPrefab; set => sectionDotPrefab = value; }
        [SerializeField] private RectTransform sectionDotPrefab;
        public Image FillImage { get => fillImage; set => fillImage = value; }
        [SerializeField] private Image fillImage;

        private float tabCount;

        public virtual void Display(UserTab tab)
        {
            var nonImageContent = tab.Encounter.Data.Content.NonImageContent;
            if (tabCount == 0)
                tabCount = nonImageContent.GetTabCount();
            var tabNumber = nonImageContent.GetCurrentTabNumber();

            FillImage.fillAmount = tabNumber / tabCount;
        }

        public virtual void Display(UserEncounter userEncounter)
        {
            var nonImageContent = userEncounter.Data.Content.NonImageContent;
            var sections = nonImageContent.Sections;
            
            var totalTabCount = nonImageContent.GetTabCount();
            var tabs = 0;

            var rect = ((RectTransform)transform).rect;

            for (var i = 0; i < sections.Count; i++) {
                var sectionDot = Instantiate(SectionDotPrefab, transform);
                sectionDot.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.height);
                sectionDot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.height);
                var position = rect.width * tabs / totalTabCount;
                var anchoredPosition = sectionDot.anchoredPosition;
                anchoredPosition.x = position;
                sectionDot.anchoredPosition = anchoredPosition;

                tabs += sections[i].Value.Tabs.Count;
            }
        }
    }
}
