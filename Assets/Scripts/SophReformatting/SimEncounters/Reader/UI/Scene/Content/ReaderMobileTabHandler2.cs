using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.UI;
using System.Collections;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabContent
    {
        public MonoBehaviour Behaviour { get; }
        public RectTransform RectTransform => (RectTransform)Behaviour.transform;
        public UserTab Tab { get; set; }

        public TabContent(MonoBehaviour behaviour) => Behaviour = behaviour;
    }

    public class ReaderMobileTabHandler2 : ReaderCompletableEncounterHandler
    {
        public MonoBehaviour TabContent1 { get => tabContent1; set => tabContent1 = value; }
        [SerializeField] private MonoBehaviour tabContent1;
        public MonoBehaviour TabContent2 { get => tabContent2; set => tabContent2 = value; }
        [SerializeField] private MonoBehaviour tabContent2;
        public MonoBehaviour TabContent3 { get => tabContent3; set => tabContent3 = value; }
        [SerializeField] private MonoBehaviour tabContent3;
        public MonoBehaviour TabContent4 { get => tabContent4; set => tabContent4 = value; }
        [SerializeField] private MonoBehaviour tabContent4;

        protected TabContent[] Contents { get; } = new TabContent[4];

        protected override void Awake()
        {
            Contents[0] = new TabContent(tabContent1);
            Contents[1] = new TabContent(tabContent2);
            Contents[2] = new TabContent(tabContent3);
            Contents[3] = new TabContent(tabContent4);

            Current = Contents[0];
            Register(Current.Behaviour);
            base.Awake();
        }

        protected ICurve Curve { get; set; }
        [Inject] public virtual void Inject(ICurve curve) => Curve = curve;

        protected TabContent Current { get; set; }
        protected TabContent Next { get; set; }
        protected TabContent Last { get; set; }
        protected TabContent Leaving { get; set; }

        protected virtual OrderedCollection<Tab> Tabs { get; set; }
        public override void Display(UserSection userSection)
        {
            Tabs = userSection.Data.Tabs;
            base.Display(userSection);
        }

        public override void Display(UserTab userTab)
        {
            if (Current.Tab != null)
                ChangeTabs(userTab);

            Current.Tab = userTab;

            base.Display(userTab);
        }


        private Coroutine currentCoroutine;
        protected virtual void ChangeTabs(UserTab userTab)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            if (Leaving != null)
                Destroy(Leaving.gameObject);

            Leaving = Current;
            Current = Instantiate(TabContentPrefab, transform);
            CurrentTabRectTransform.SetSiblingIndex(0);

            Deregister(Leaving);
            Register(Current);

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
            var moveAmount = Curve.GetCurveX(-LeavingTabRectTransform.anchorMin.x);

            while (moveAmount < 1) {
                moveAmount += Time.deltaTime / MoveTime;
                SetMoveAmountForward(moveAmount);
                yield return null;
            }

            SetMoveAmountForward(moveAmount);
            Destroy(Leaving.gameObject);
        }

        protected virtual void SetMoveAmountForward(float moveAmount)
        {
            moveAmount = Mathf.Clamp01(moveAmount);
            moveAmount = Curve.GetCurveY(moveAmount);

            LeavingTabRectTransform.anchorMin = new Vector2(0 - moveAmount, CurrentTabRectTransform.anchorMin.y);
            LeavingTabRectTransform.anchorMax = new Vector2(1 - moveAmount, CurrentTabRectTransform.anchorMax.y);
            CurrentTabRectTransform.anchorMin = new Vector2(1 - moveAmount, CurrentTabRectTransform.anchorMin.y);
            CurrentTabRectTransform.anchorMax = new Vector2(2 - moveAmount, CurrentTabRectTransform.anchorMax.y);
        }

        protected virtual IEnumerator ShiftTabBackward()
        {
            var moveAmount = Curve.GetCurveX(LeavingTabRectTransform.anchorMin.x);

            while (moveAmount < 1) {
                moveAmount += Time.deltaTime / MoveTime;
                SetMoveAmountBackward(moveAmount);
                yield return null;
            }

            SetMoveAmountBackward(moveAmount);
            Destroy(Leaving.gameObject);
        }

        protected virtual void SetMoveAmountBackward(float moveAmount)
        {
            moveAmount = Mathf.Clamp01(moveAmount);
            moveAmount = Curve.GetCurveY(moveAmount);

            LeavingTabRectTransform.anchorMin = new Vector2(0 + moveAmount, CurrentTabRectTransform.anchorMin.y);
            LeavingTabRectTransform.anchorMax = new Vector2(1 + moveAmount, CurrentTabRectTransform.anchorMax.y);
            CurrentTabRectTransform.anchorMin = new Vector2(-1 + moveAmount, CurrentTabRectTransform.anchorMin.y);
            CurrentTabRectTransform.anchorMax = new Vector2(0 + moveAmount, CurrentTabRectTransform.anchorMax.y);
        }
    }
}
