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

        protected ISelectedListener<EncounterSelectedEventArgs> EncounterSelector { get; set; }
        protected ISelectedListener<TabSelectedEventArgs> TabSelector { get; set; }
        [Inject]
        public virtual void Inject(
            ISelectedListener<EncounterSelectedEventArgs> encounterSelector, 
            ISelectedListener<TabSelectedEventArgs> tabSelector)
        {
            EncounterSelector = encounterSelector;
            TabSelector = tabSelector;
        }
        protected virtual void Start()
        {
            EncounterSelector.Selected += OnEncounterSelected;
            if (EncounterSelector.CurrentValue != null)
                OnEncounterSelected(EncounterSelector, EncounterSelector.CurrentValue);

            TabSelector.Selected += OnTabSelected;
            if (TabSelector.CurrentValue != null)
                OnTabSelected(TabSelector, TabSelector.CurrentValue);
        }

        protected EncounterNonImageContent NonImageContent { get; set; }
        private float tabCount;
        public virtual void OnEncounterSelected(object sender, EncounterSelectedEventArgs eventArgs)
        {
            NonImageContent = eventArgs.Encounter.Content.NonImageContent;
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
            EncounterSelector.Selected -= OnEncounterSelected;
            TabSelector.Selected -= OnTabSelected;
        }
    }
}
