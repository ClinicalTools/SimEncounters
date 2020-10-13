using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSectionHandler2 : ReaderCompletableEncounterHandler
    {
        public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }
        [SerializeField] private CanvasGroup canvasGroup;
        public MonoBehaviour SectionContent1 { get => sectionContent1; set => sectionContent1 = value; }
        [SerializeField] private MonoBehaviour sectionContent1;
        public MonoBehaviour SectionContent2 { get => sectionContent2; set => sectionContent2 = value; }
        [SerializeField] private MonoBehaviour sectionContent2;
        public MonoBehaviour SectionContent3 { get => sectionContent3; set => sectionContent3 = value; }
        [SerializeField] private MonoBehaviour sectionContent3;
        public MonoBehaviour SectionContent4 { get => sectionContent4; set => sectionContent4 = value; }
        [SerializeField] private MonoBehaviour sectionContent4;

        protected SectionContent[] Contents { get; } = new SectionContent[4];

        protected override void Awake()
        {
            Contents[0] = new SectionContent(sectionContent1);
            Contents[1] = new SectionContent(sectionContent2);
            Contents[2] = new SectionContent(sectionContent3);
            Contents[3] = new SectionContent(sectionContent4);

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

        protected SectionContent Current { get; set; }
        protected SectionContent Next { get; set; }
        protected SectionContent Last { get; set; }
        protected SectionContent Leaving { get; set; }

        protected virtual OrderedCollection<UserSection> Sections { get; set; }
        public override void Display(UserEncounter userEncounter)
        {
            Sections = userEncounter.Sections;
            ClearCurrent();

            base.Display(userEncounter);
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

        public override void Display(UserSection userSection)
        {
            if (Current?.Section == userSection) {
                ChildSelectedSection = false;
                return;
            }

            if (!ChildSelectedSection)
                ClearCurrent();
            else
                ChildSelectedSection = false;

            HandleSectionStuff(userSection);
            base.Display(userSection);
        }

        protected bool ChildSelectedSection { get; set; }
        protected override void OnSectionSelected(object sender, UserSectionSelectedEventArgs e)
        {
            ChildSelectedSection = true;
            base.OnSectionSelected(sender, e);
        }

        protected virtual void HandleSectionStuff(UserSection currentSection)
        {
            var sectionIndex = Sections.IndexOf(currentSection);
            var lastSection = (sectionIndex > 0) ? Sections[sectionIndex - 1].Value : null;
            var nextSection = (sectionIndex < Sections.Count - 1) ? Sections[sectionIndex + 1].Value : null;

            ClearCurrent();
            Last = null;
            Next = null;

            var unusedContent = new Stack<SectionContent>();
            foreach (var sectionContent in Contents) {
                if (sectionContent.Section == null)
                    unusedContent.Push(sectionContent);
                else if (sectionContent.Section == lastSection)
                    Last = sectionContent;
                else if (sectionContent.Section == currentSection)
                    Current = sectionContent;
                else if (sectionContent.Section == nextSection)
                    Next = sectionContent;
                else if (sectionContent == Leaving)
                    continue;
                else
                    unusedContent.Push(sectionContent);
            }

            if (Current == null) {
                Current = unusedContent.Pop();
                Current.Section = currentSection;
            }
            if (lastSection != null && Last == null) {
                Last = unusedContent.Pop();
                Last.Section = lastSection;
            }
            if (nextSection != null && Next == null) {
                Next = unusedContent.Pop();
                Next.Section = nextSection;
            }

            Last?.SetCurrentTab();
            Current.SetCurrentTab();
            Next?.SetFirstTab();

            Register(Current.Behaviour);

            TabDraw();
        }

        private Coroutine currentCoroutine;
        protected virtual void TabDraw()
        {
            foreach (var sectionContent in Contents)
                sectionContent.GameObject.SetActive(sectionContent == Current || sectionContent == Leaving);

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
            else if (Sections.IndexOf(Leaving.Section) < Sections.IndexOf(Current.Section))
                return ShiftForward(Leaving);
            else
                return ShiftBackward(Leaving);
        }

        protected IEnumerator ShiftForward(SectionContent leavingContent)
        {
            SwipeManager.DisableSwipe();
            yield return Curve.ShiftForward(leavingContent.RectTransform, Current.RectTransform);
            SwipeManager.ReenableSwipe();
        }
        protected IEnumerator ShiftBackward(SectionContent leavingContent)
        {
            SwipeManager.DisableSwipe();
            yield return Curve.ShiftBackward(leavingContent.RectTransform, Current.RectTransform);
            SwipeManager.ReenableSwipe();
        }

        SwipeParameter SwipeParamater { get; set; }
        protected virtual void InitializeSwipeParamaters()
        {
            SwipeParamater = new SwipeParameter();
            SwipeParamater.AngleRanges.Add(new AngleRange(-30, 30));
            SwipeParamater.AngleRanges.Add(new AngleRange(150, 210));
            SwipeParamater.OnSwipeStart += SwipeStart;
            SwipeParamater.OnSwipeUpdate += SwipeUpdate;
            SwipeParamater.OnSwipeEnd += SwipeEnd;
        }

        private void SwipeStart(Swipe obj)
        {
            DragOverrideScript.DragAllowed = false;
            CanvasGroup.blocksRaycasts = false;
            SwipeUpdate(obj);
        }
        private void SwipeUpdate(Swipe obj)
        {
            var dist = (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
            if (dist > 0)
                RightSwipeUpdate(Mathf.Clamp01(dist));
            else
                LeftSwipeUpdate(Mathf.Clamp01(-dist));
        }

        private bool swipingLeft, swipingRight;
        private void RightSwipeUpdate(float dist)
        {
            if (Next != null)
                Next.GameObject.SetActive(false);
            if (Last == null || Current.Section.Data.CurrentTabIndex != 0) {
                Curve.SetPosition(Current.RectTransform);
                return;
            }
            swipingRight = true;
            Last.GameObject.SetActive(true);
            Curve.SetMoveAmountBackward(Current.RectTransform, Last.RectTransform, dist);
        }
        private void LeftSwipeUpdate(float dist)
        {
            if (Last != null)
                Last.GameObject.SetActive(false);
            if (Next == null || Current.Section.Data.CurrentTabIndex + 1 != Current.Section.Tabs.Count) {
                Curve.SetPosition(Current.RectTransform);
                return;
            }
            swipingLeft = true;
            Next.GameObject.SetActive(true);
            Curve.SetMoveAmountForward(Current.RectTransform, Next.RectTransform, dist);
        }
        private void SwipeEnd(Swipe obj)
        {
            DragOverrideScript.DragAllowed = true;
            CanvasGroup.blocksRaycasts = true;

            var dist = (obj.LastPosition.x - obj.StartPosition.x) / Screen.width;
            if (swipingRight && dist > 0 && Last != null && Current.Section.Data.CurrentTabIndex == 0) {
                if (dist > .5f || obj.Velocity.x / Screen.dpi > 1.5f)
                    OnSectionSelected(this, new UserSectionSelectedEventArgs(Last.Section));
                else
                    StartCoroutine(ShiftForward(Last));
            } else if (swipingLeft && dist < 0 && Next != null && Current.Section.Data.CurrentTabIndex + 1 == Current.Section.Tabs.Count) {
                if (dist < -.5f || obj.Velocity.x / Screen.dpi < -1.5f)
                    OnSectionSelected(this, new UserSectionSelectedEventArgs(Next.Section));
                else
                    StartCoroutine(ShiftBackward(Next));
            }

            swipingLeft = false;
            swipingRight = false;
        }
    }
}
