using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.UI;
using System.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSectionHandler : ReaderCompletableEncounterHandler
    {
        public MonoBehaviour SectionContentPrefab { get => sectionContentPrefab; set => sectionContentPrefab = value; }
        [SerializeField] private MonoBehaviour sectionContentPrefab;
        public MonoBehaviour SectionContentDefault { get => sectionContentDefault; set => sectionContentDefault = value; }
        [SerializeField] private MonoBehaviour sectionContentDefault;


        protected override void Awake()
        {
            CurrentSectionContent = SectionContentDefault;
            Register(CurrentSectionContent);
            base.Awake();
        }

        protected ICurve Curve { get; set; }
        [Inject] public virtual void Inject(ICurve curve) => Curve = curve;

        protected MonoBehaviour CurrentSectionContent { get; set; }
        protected RectTransform CurrentSectionRectTransform => (RectTransform)CurrentSectionContent.transform;
        protected MonoBehaviour PreviousSectionContent { get; set; }
        protected RectTransform PreviousSectionRectTransform => (RectTransform)PreviousSectionContent.transform;

        protected virtual OrderedCollection<Section> Sections { get; set; }
        protected virtual UserSection CurrentSection { get; set; }
        public override void Display(UserEncounter userEncounter)
        {
            Sections = userEncounter.Data.Content.NonImageContent.Sections;
            base.Display(userEncounter);
        }

        private bool showSectionAnimation = false;
        public override void Display(UserSection userSection)
        {
            if (showSectionAnimation) {
                ChangeSections(userSection);
                showSectionAnimation = false;
            }

            CurrentSection = userSection;

            base.Display(userSection);
        }

        protected override void OnSectionSelected(object sender, UserSectionSelectedEventArgs e)
        {
            showSectionAnimation = true;
            base.OnSectionSelected(sender, e);
        }

        private Coroutine currentCoroutine;
        protected virtual void ChangeSections(UserSection userSection)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            if (PreviousSectionContent != null)
                Destroy(PreviousSectionContent.gameObject);

            PreviousSectionContent = CurrentSectionContent;
            CurrentSectionContent = Instantiate(SectionContentPrefab, transform);
            CurrentSectionRectTransform.SetSiblingIndex(0);

            Deregister(PreviousSectionContent);
            Register(CurrentSectionContent);

            IEnumerator shiftSectionRoutine;
            if (Sections.IndexOf(userSection.Data) > Sections.IndexOf(CurrentSection.Data))
                shiftSectionRoutine = ShiftSectionForward();
            else
                shiftSectionRoutine = ShiftSectionBackward();
            currentCoroutine = StartCoroutine(shiftSectionRoutine);
        }

        private const float MoveTime = .5f;
        protected virtual IEnumerator ShiftSectionForward()
        {
            var moveAmount = Curve.GetCurveX(-PreviousSectionRectTransform.anchorMin.x);

            while (moveAmount < 1) {
                moveAmount += Time.deltaTime / MoveTime;
                SetMoveAmountForward(moveAmount);
                yield return null;
            }

            SetMoveAmountForward(moveAmount);
            Destroy(PreviousSectionContent.gameObject);
        }

        protected virtual void SetMoveAmountForward(float moveAmount)
        {
            moveAmount = Mathf.Clamp01(moveAmount);
            moveAmount = Curve.GetCurveY(moveAmount);

            PreviousSectionRectTransform.anchorMin = new Vector2(0 - moveAmount, CurrentSectionRectTransform.anchorMin.y);
            PreviousSectionRectTransform.anchorMax = new Vector2(1 - moveAmount, CurrentSectionRectTransform.anchorMax.y);
            CurrentSectionRectTransform.anchorMin = new Vector2(1 - moveAmount, CurrentSectionRectTransform.anchorMin.y);
            CurrentSectionRectTransform.anchorMax = new Vector2(2 - moveAmount, CurrentSectionRectTransform.anchorMax.y);
        }

        protected virtual IEnumerator ShiftSectionBackward()
        {
            var moveAmount = Curve.GetCurveX(PreviousSectionRectTransform.anchorMin.x);

            while (moveAmount < 1) {
                moveAmount += Time.deltaTime / MoveTime;
                SetMoveAmountBackward(moveAmount);
                yield return null;
            }

            SetMoveAmountBackward(moveAmount);
            Destroy(PreviousSectionContent.gameObject);
        }

        protected virtual void SetMoveAmountBackward(float moveAmount)
        {
            moveAmount = Mathf.Clamp01(moveAmount);
            moveAmount = Curve.GetCurveY(moveAmount);

            PreviousSectionRectTransform.anchorMin = new Vector2(0 + moveAmount, CurrentSectionRectTransform.anchorMin.y);
            PreviousSectionRectTransform.anchorMax = new Vector2(1 + moveAmount, CurrentSectionRectTransform.anchorMax.y);
            CurrentSectionRectTransform.anchorMin = new Vector2(-1 + moveAmount, CurrentSectionRectTransform.anchorMin.y);
            CurrentSectionRectTransform.anchorMax = new Vector2(0 + moveAmount, CurrentSectionRectTransform.anchorMax.y);
        }
    }
}
