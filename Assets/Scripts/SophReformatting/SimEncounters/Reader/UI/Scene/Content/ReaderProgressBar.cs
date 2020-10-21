using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderProgressBar : MonoBehaviour
    {
        public RectTransform SectionDotPrefab { get => sectionDotPrefab; set => sectionDotPrefab = value; }
        [SerializeField] private RectTransform sectionDotPrefab;
        public Image FillImage { get => fillImage; set => fillImage = value; }
        [SerializeField] private Image fillImage;

        protected ISelectedListener<Encounter> EncounterSelector { get; set; }
        protected ISelectedListener<TabSelectedEventArgs> TabSelector { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<Encounter> encounterSelector, 
            ISelectedListener<TabSelectedEventArgs> tabSelector)
        {
            EncounterSelector = encounterSelector;
            EncounterSelector.AddSelectedListener(OnEncounterSelected);
            TabSelector = tabSelector;
            TabSelector.AddSelectedListener(OnTabSelected);
        }

        protected EncounterNonImageContent NonImageContent { get; set; }
        private float tabCount;
        public virtual void OnEncounterSelected(object sender, Encounter encounter)
        {
            NonImageContent = encounter.Content.NonImageContent;
            var sections = NonImageContent.Sections;

            tabCount = NonImageContent.GetTabCount();
            var tabs = 0;

            var rect = ((RectTransform)transform).rect;

            for (var i = 0; i < sections.Count; i++) {
                var sectionDot = Instantiate(SectionDotPrefab, transform);
                var position = 1f * tabs / tabCount;
                sectionDot.anchorMin = new Vector2(position, 0);
                sectionDot.anchorMax = new Vector2(position, 1);
                sectionDot.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.height);

                sectionDot.anchoredPosition = Vector2.zero;

                tabs += sections[i].Value.Tabs.Count;
            }
        }

        protected virtual void OnTabSelected(object sender, TabSelectedEventArgs eventArgs)
            => FillImage.fillAmount = NonImageContent.GetCurrentTabNumber() / tabCount;
        
        protected virtual void OnDestroy()
        {
            EncounterSelector?.RemoveSelectedListener(OnEncounterSelected);
            TabSelector?.RemoveSelectedListener(OnTabSelected);
        }

    }
}
