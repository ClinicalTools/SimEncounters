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
                var position = 1f * tabs / totalTabCount;
                sectionDot.anchorMin = new Vector2(position, 0);
                sectionDot.anchorMax = new Vector2(position, 1);
                sectionDot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.height);

                sectionDot.anchoredPosition = Vector2.zero;

                tabs += sections[i].Value.Tabs.Count;
            }
        }
    }
}
