using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileTabHandler : ReaderCompletableEncounterHandler
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

            base.Awake();
        }

        protected SwipeManager SwipeManager { get; set; }
        protected IShifter Curve { get; set; }
        [Inject]
        public virtual void Inject(IShifter curve, SwipeManager swipeManager)
        {
            Curve = curve;
            SwipeManager = swipeManager;
        }

        protected TabContent Current { get; set; }
        protected TabContent Next { get; set; }
        protected TabContent Last { get; set; }
        protected TabContent Leaving { get; set; }

        protected virtual OrderedCollection<UserTab> Tabs { get; set; }
        public override void Display(UserSectionSelectedEventArgs eventArgs)
        {
            Tabs = eventArgs.SelectedSection.Tabs;
            ClearCurrent();

            base.Display(eventArgs);
        }

        protected virtual void OnEnable()
        {
            if (SwipeParamater == null)
                InitializeSwipeParamaters();
            SwipeManager.AddSwipeAction(SwipeParamater);

            ClearCurrent();
        }
        protected virtual void OnDisable() => SwipeManager.RemoveSwipeAction(SwipeParamater);

        protected virtual void ClearCurrent()
        {
            Leaving = Current;
            if (Current != null)
                Deregister(Current.Behaviour);
            Current = null;
        }

        public override void Display(UserTabSelectedEventArgs eventArgs)
        {
            if (Current?.Tab == eventArgs.SelectedTab)
                return;

            HandleTabStuff(eventArgs);
            base.Display(eventArgs);
        }

        protected virtual void HandleTabStuff(UserTabSelectedEventArgs eventArgs)
        {
            var tabIndex = Tabs.IndexOf(eventArgs.SelectedTab);
            var lastTab = (tabIndex > 0) ? Tabs[tabIndex - 1].Value : null;
            var nextTab = (tabIndex < Tabs.Count - 1) ? Tabs[tabIndex + 1].Value : null;

            ClearCurrent();
            Last = null;
            Next = null;

            var unusedContent = new Stack<TabContent>();
            foreach (var tabContent in Contents) {
                if (tabContent.Tab == null)
                    unusedContent.Push(tabContent);
                else if (tabContent.Tab == lastTab)
                    Last = tabContent;
                else if (tabContent.Tab == eventArgs.SelectedTab)
                    Current = tabContent;
                else if (tabContent.Tab == nextTab)
                    Next = tabContent;
                else if (tabContent == Leaving)
                    continue;
                else
                    unusedContent.Push(tabContent);
            }

            if (Current == null)
                Current = unusedContent.Pop();
            if (lastTab != null && Last == null)
                Last = unusedContent.Pop();
            if (nextTab != null && Next == null)
                Next = unusedContent.Pop();

            Current.ChangeTab(eventArgs);
            Last?.ChangeTab(new UserTabSelectedEventArgs(lastTab, ChangeType.Inactive));
            Next?.ChangeTab(new UserTabSelectedEventArgs(nextTab, ChangeType.Inactive));

            Register(Current.Behaviour);

            TabDraw();
        }

        private Coroutine currentCoroutine;
        protected virtual void TabDraw()
        {
            foreach (var tabContent in Contents)
                tabContent.GameObject.SetActive(tabContent == Current || tabContent == Leaving);

            if (currentCoroutine != null) {
                StopCoroutine(currentCoroutine);
                SwipeManager.ReenableSwipe();
            }

            IEnumerator shiftSectionRoutine = GetShiftRoutine();
            if (shiftSectionRoutine != null)
                currentCoroutine = StartCoroutine(shiftSectionRoutine);
            else
                Curve.SetPosition(Current.RectTransform);
        }

        protected IEnumerator GetShiftRoutine()
        {
            if (!gameObject.activeInHierarchy || Leaving == null)
                return null;
            else if (Tabs.IndexOf(Leaving.Tab) < Tabs.IndexOf(Current.Tab))
                return ShiftForward(Leaving);
            else
                return ShiftBackward(Leaving);
        }

        protected IEnumerator ShiftForward(TabContent leavingContent)
            => Shift(Curve.ShiftForward(leavingContent.RectTransform, Current.RectTransform));
        protected IEnumerator ShiftBackward(TabContent leavingContent)
            => Shift(Curve.ShiftBackward(leavingContent.RectTransform, Current.RectTransform));
        protected IEnumerator Shift(IEnumerator enumerator)
        {
            SwipeManager.DisableSwipe();
            yield return enumerator;
            SwipeManager.ReenableSwipe();
        }

        protected SwipeParameter SwipeParamater { get; set; }
        protected virtual void InitializeSwipeParamaters()
        {
            SwipeParamater = new SwipeParameter();
            SwipeParamater.AngleRanges.Add(new AngleRange(-30, 30));
            SwipeParamater.AngleRanges.Add(new AngleRange(150, 210));
            SwipeParamater.OnSwipeStart += SwipeStart;
            SwipeParamater.OnSwipeUpdate += SwipeUpdate;
            SwipeParamater.OnSwipeEnd += SwipeEnd;
        }

        private void SwipeStart(Swipe obj) => SwipeUpdate(obj);
        private void SwipeUpdate(Swipe obj)
        {
            var dist = (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
            if (dist > 0)
                RightSwipeUpdate(Mathf.Clamp01(dist));
            else
                LeftSwipeUpdate(Mathf.Clamp01(-dist));
        }

        private void RightSwipeUpdate(float dist)
        {
            if (Next != null)
                Next.GameObject.SetActive(false);
            if (Last == null)
                return;
            Last.GameObject.SetActive(true);
            Curve.SetMoveAmountBackward(Current.RectTransform, Last.RectTransform, dist);
        }
        private void LeftSwipeUpdate(float dist)
        {
            if (Last != null)
                Last.GameObject.SetActive(false);
            if (Next == null)
                return;
            Next.GameObject.SetActive(true);
            Curve.SetMoveAmountForward(Current.RectTransform, Next.RectTransform, dist);
        }
        private void SwipeEnd(Swipe obj)
        {
            SwipeUpdate(obj);

            var dist = (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
            if (dist > 0 && Last != null) {
                if (dist > .5f || obj.Velocity.x / Screen.dpi > 1.5f)
                    OnTabSelected(this, new UserTabSelectedEventArgs(Last.Tab, ChangeType.Previous));
                else
                    currentCoroutine = StartCoroutine(ShiftForward(Last));
            } else if (dist < 0 && Next != null) {
                if (dist < -.5f || obj.Velocity.x / Screen.dpi < -1.5f)
                    OnTabSelected(this, new UserTabSelectedEventArgs(Next.Tab, ChangeType.Next));
                else
                    currentCoroutine = StartCoroutine(ShiftBackward(Next));
            }
        }
    }
}
