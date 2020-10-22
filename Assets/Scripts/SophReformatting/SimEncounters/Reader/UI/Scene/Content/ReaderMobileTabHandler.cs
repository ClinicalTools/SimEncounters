using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileTabHandler : MonoBehaviour
    {
        public UserTabDrawer TabDrawer1 { get => tabContent1; set => tabContent1 = value; }
        [SerializeField] private UserTabDrawer tabContent1;
        public UserTabDrawer TabDrawer2 { get => tabDrawer2; set => tabDrawer2 = value; }
        [SerializeField] private UserTabDrawer tabDrawer2;
        public UserTabDrawer TabDrawer3 { get => tabDrawer3; set => tabDrawer3 = value; }
        [SerializeField] private UserTabDrawer tabDrawer3;
        public UserTabDrawer TabDrawer4 { get => tabDrawer4; set => tabDrawer4 = value; }
        [SerializeField] private UserTabDrawer tabDrawer4;

        protected UserTabDrawer[] Contents { get; } = new UserTabDrawer[4];

        protected virtual void Awake()
        {
            Contents[0] = TabDrawer1;
            Contents[1] = TabDrawer2;
            Contents[2] = TabDrawer3;
            Contents[3] = TabDrawer4;
        }

        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        protected SwipeManager SwipeManager { get; set; }
        protected IShifter Curve { get; set; }
        [Inject]
        public virtual void Inject(
            IShifter curve,
            SwipeManager swipeManager,
            ISelector<UserSectionSelectedEventArgs> userSectionSelector,
            ISelector<UserTabSelectedEventArgs> userTabSelector)
        {
            UserSectionSelector = userSectionSelector;
            UserTabSelector = userTabSelector;
            Curve = curve;
            SwipeManager = swipeManager;
        }
        protected virtual void Start()
        {
            UserSectionSelector.AddSelectedListener(OnSectionSelected);
            UserTabSelector.AddSelectedListener(OnTabSelected);
        }

        protected UserTabDrawer Current { get; set; }
        protected UserTabDrawer Next { get; set; }
        protected UserTabDrawer Last { get; set; }
        protected UserTabDrawer Leaving { get; set; }

        protected virtual OrderedCollection<UserTab> Tabs { get; set; }
        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            Tabs = eventArgs.SelectedSection.Tabs;
            ClearCurrent();
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
            Current = null;
        }

        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (Current != null && Current.Tab == eventArgs.SelectedTab)
                return;

            HandleTabStuff(sender, eventArgs);
        }

        protected virtual void HandleTabStuff(object sender, UserTabSelectedEventArgs eventArgs)
        {
            var tabIndex = Tabs.IndexOf(eventArgs.SelectedTab);
            var lastTab = (tabIndex > 0) ? Tabs[tabIndex - 1].Value : null;
            var nextTab = (tabIndex < Tabs.Count - 1) ? Tabs[tabIndex + 1].Value : null;

            ClearCurrent();
            Last = null;
            Next = null;

            var unusedContent = new Stack<UserTabDrawer>();
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

            Current.ChangeTab(sender, eventArgs);
            if (Last != null)
                Last.ChangeTab(this, new UserTabSelectedEventArgs(lastTab, ChangeType.Inactive));
            if (Next != null)
                Next.ChangeTab(this, new UserTabSelectedEventArgs(nextTab, ChangeType.Inactive));

            TabDraw();
        }

        private Coroutine currentCoroutine;
        protected virtual void TabDraw()
        {
            foreach (var tabContent in Contents)
                tabContent.gameObject.SetActive(tabContent == Current || tabContent == Leaving);

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

        protected IEnumerator ShiftForward(UserTabDrawer leavingContent)
            => Shift(Curve.ShiftForward(leavingContent.RectTransform, Current.RectTransform));
        protected IEnumerator ShiftBackward(UserTabDrawer leavingContent)
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
                Next.gameObject.SetActive(false);
            if (Last == null)
                return;
            Last.gameObject.SetActive(true);
            Curve.SetMoveAmountBackward(Current.RectTransform, Last.RectTransform, dist);
        }
        private void LeftSwipeUpdate(float dist)
        {
            if (Last != null)
                Last.gameObject.SetActive(false);
            if (Next == null)
                return;
            Next.gameObject.SetActive(true);
            Curve.SetMoveAmountForward(Current.RectTransform, Next.RectTransform, dist);
        }
        private void SwipeEnd(Swipe obj)
        {
            SwipeUpdate(obj);

            var dist = (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
            if (dist > 0 && Last != null) {
                if (dist > .5f || obj.Velocity.x / Screen.dpi > 1.5f)
                    UserTabSelector.Select(this, new UserTabSelectedEventArgs(Last.Tab, ChangeType.Previous));
                else
                    currentCoroutine = StartCoroutine(ShiftForward(Last));
            } else if (dist < 0 && Next != null) {
                if (dist < -.5f || obj.Velocity.x / Screen.dpi < -1.5f)
                    UserTabSelector.Select(this, new UserTabSelectedEventArgs(Next.Tab, ChangeType.Next));
                else
                    currentCoroutine = StartCoroutine(ShiftBackward(Next));
            }
        }
    }
}
