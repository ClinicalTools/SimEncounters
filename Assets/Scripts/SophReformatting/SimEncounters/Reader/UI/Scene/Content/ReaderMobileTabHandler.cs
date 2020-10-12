using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.UI;
using System.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileTabHandler : ReaderCompletableEncounterHandler
    {
        public MonoBehaviour TabContentPrefab { get => tabContentPrefab; set => tabContentPrefab = value; }
        [SerializeField] private MonoBehaviour tabContentPrefab;
        public MonoBehaviour TabContentDefault { get => tabContentDefault; set => tabContentDefault = value; }
        [SerializeField] private MonoBehaviour tabContentDefault;


        protected override void Awake()
        {
            CurrentTabContent = TabContentDefault;
            Register(CurrentTabContent);
            base.Awake();
        }

        protected ICurve Curve { get; set; }
        [Inject] public virtual void Inject(ICurve curve) => Curve = curve;

        protected MonoBehaviour CurrentTabContent { get; set; }
        protected RectTransform CurrentTabRectTransform => (RectTransform)CurrentTabContent.transform;
        protected MonoBehaviour PreviousTabContent { get; set; }
        protected RectTransform PreviousTabRectTransform => (RectTransform)PreviousTabContent.transform;

        protected virtual OrderedCollection<Tab> Tabs { get; set; }
        protected virtual UserTab CurrentTab { get; set; }
        public override void Display(UserSection userSection)
        {
            Tabs = userSection.Data.Tabs;
            base.Display(userSection);
        }

        public override void Display(UserTab userTab)
        {
            if (CurrentTab != null)
                ChangeTabs(userTab);

            CurrentTab = userTab;

            base.Display(userTab);
        }


        private Coroutine currentCoroutine;
        protected virtual void ChangeTabs(UserTab userTab)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            if (PreviousTabContent != null)
                Destroy(PreviousTabContent.gameObject);

            PreviousTabContent = CurrentTabContent;
            CurrentTabContent = Instantiate(TabContentPrefab, transform);
            CurrentTabRectTransform.SetSiblingIndex(0);

            Deregister(PreviousTabContent);
            Register(CurrentTabContent);

            IEnumerator shiftSectionRoutine;
            if (Tabs.IndexOf(userTab.Data) > Tabs.IndexOf(CurrentTab.Data))
                shiftSectionRoutine = ShiftTabForward();
            else
                shiftSectionRoutine = ShiftTabBackward();
            currentCoroutine = StartCoroutine(shiftSectionRoutine);
        }

        private const float MoveTime = .5f;
        protected virtual IEnumerator ShiftTabForward()
        {
            var moveAmount = Curve.GetCurveX(-PreviousTabRectTransform.anchorMin.x);

            while (moveAmount < 1) {
                moveAmount += Time.deltaTime / MoveTime;
                SetMoveAmountForward(moveAmount);
                yield return null;
            }

            SetMoveAmountForward(moveAmount);
            Destroy(PreviousTabContent.gameObject);
        }

        protected virtual void SetMoveAmountForward(float moveAmount)
        {
            moveAmount = Mathf.Clamp01(moveAmount);
            moveAmount = Curve.GetCurveY(moveAmount);

            PreviousTabRectTransform.anchorMin = new Vector2(0 - moveAmount, CurrentTabRectTransform.anchorMin.y);
            PreviousTabRectTransform.anchorMax = new Vector2(1 - moveAmount, CurrentTabRectTransform.anchorMax.y);
            CurrentTabRectTransform.anchorMin = new Vector2(1 - moveAmount, CurrentTabRectTransform.anchorMin.y);
            CurrentTabRectTransform.anchorMax = new Vector2(2 - moveAmount, CurrentTabRectTransform.anchorMax.y);
        }

        protected virtual IEnumerator ShiftTabBackward()
        {
            var moveAmount = Curve.GetCurveX(PreviousTabRectTransform.anchorMin.x);

            while (moveAmount < 1) {
                moveAmount += Time.deltaTime / MoveTime;
                SetMoveAmountBackward(moveAmount);
                yield return null;
            }

            SetMoveAmountBackward(moveAmount);
            Destroy(PreviousTabContent.gameObject);
        }

        protected virtual void SetMoveAmountBackward(float moveAmount)
        {
            moveAmount = Mathf.Clamp01(moveAmount);
            moveAmount = Curve.GetCurveY(moveAmount);

            PreviousTabRectTransform.anchorMin = new Vector2(0 + moveAmount, CurrentTabRectTransform.anchorMin.y);
            PreviousTabRectTransform.anchorMax = new Vector2(1 + moveAmount, CurrentTabRectTransform.anchorMax.y);
            CurrentTabRectTransform.anchorMin = new Vector2(-1 + moveAmount, CurrentTabRectTransform.anchorMin.y);
            CurrentTabRectTransform.anchorMax = new Vector2(0 + moveAmount, CurrentTabRectTransform.anchorMax.y);
        }
    }
}
