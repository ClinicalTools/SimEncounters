using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
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

            base.Awake();
        }

        protected IShifter Curve { get; set; }
        [Inject] public virtual void Inject(IShifter curve) => Curve = curve;

        protected TabContent Current { get; set; }
        protected TabContent Next { get; set; }
        protected TabContent Last { get; set; }
        protected TabContent Leaving { get; set; }

        protected virtual OrderedCollection<UserTab> Tabs { get; set; }
        public override void Display(UserSection userSection)
        {
            Tabs = userSection.Tabs;
            ClearCurrent();

            base.Display(userSection);
        }

        protected virtual void OnEnable() => ClearCurrent();

        protected virtual void ClearCurrent()
        {
            Leaving = Current;
            if (Current != null)
                Deregister(Current.Behaviour);
            Current = null;
        }

        public override void Display(UserTab userTab)
        {
            if (Current?.Tab == userTab)
                return;

            HandleTabStuff(userTab);
            base.Display(userTab);
        }

        protected virtual void HandleTabStuff(UserTab currentTab)
        {
            var tabIndex = Tabs.IndexOf(currentTab);
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
                else if (tabContent.Tab == currentTab)
                    Current = tabContent;
                else if (tabContent.Tab == nextTab)
                    Next = tabContent;
                else if (tabContent == Leaving)
                    continue;
                else
                    unusedContent.Push(tabContent);
            }

            if (lastTab != null && Last == null) {
                Last = unusedContent.Pop();
                Last.Tab = lastTab;
            }
            if (currentTab != null && Current == null) {
                Current = unusedContent.Pop();
                Current.Tab = currentTab;
            }
            if (nextTab != null && Next == null) {
                Next = unusedContent.Pop();
                Next.Tab = nextTab;
            }

            Register(Current.Behaviour);

            TabDraw();
        }

        private Coroutine currentCoroutine;
        protected virtual void TabDraw()
        {
            foreach (var tabContent in Contents)
                tabContent.GameObject.SetActive(tabContent == Current || tabContent == Leaving);

            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);

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
                return Curve.ShiftForward(Leaving.RectTransform, Current.RectTransform);
            else
                return Curve.ShiftBackward(Leaving.RectTransform, Current.RectTransform);
        }


        protected virtual void SwipeStuff()
        {

        }

        protected virtual void SwipeLeft()
        {

        }
        protected virtual void SwipeRight()
        {

        }
        private SwipeManager swipeManager;
        protected virtual void InitializeSidebarParamaters()
        {
            var OpenSidebarSwipeParamater = new SwipeParameter {
                AngleRange = new AngleRange(-30, 30)
            };
            //OpenSidebarSwipeParamater.OnSwipeStart += OpenSwipeStart;
            //OpenSidebarSwipeParamater.OnSwipeUpdate += OpenSwipeUpdate;
            //OpenSidebarSwipeParamater.OnSwipeEnd += OpenSwipeEnd;
            swipeManager.AddSwipeAction(OpenSidebarSwipeParamater);

            var RightSwipeParamater = new SwipeParameter {
                AngleRange = new AngleRange(150, 210)
            };
            //RightSwipeParamater.OnSwipeStart += RightSwipeStart;
            //RightSwipeParamater.OnSwipeUpdate += CloseSwipeUpdate;
            //RightSwipeParamater.OnSwipeEnd += CloseSwipeEnd;

            swipeManager.AddSwipeAction(RightSwipeParamater);
        }

        private void RightSwipeStart(Swipe obj)
        {
            Next.GameObject.SetActive(true);
            //CloseSwipeUpdate(swipe);
        }
        private void RightSwipeUpdate(Swipe obj)
        {
            obj.StartPosition.
            //BeginHidingSidebar();
            //CloseSwipeUpdate(swipe);
        }
        private void RightSwipeEnd(Swipe obj)
        {
            //BeginHidingSidebar();
            //CloseSwipeUpdate(swipe);
        }
    }
}
