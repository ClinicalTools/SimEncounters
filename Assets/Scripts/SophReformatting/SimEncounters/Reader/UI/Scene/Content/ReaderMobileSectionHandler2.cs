using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSectionHandler2 : ReaderCompletableEncounterHandler
    {
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

        protected IShifter Curve { get; set; }
        [Inject] public virtual void Inject(IShifter curve) => Curve = curve;

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

        protected virtual void OnEnable() => ClearCurrent();

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
                return Curve.ShiftForward(Leaving.RectTransform, Current.RectTransform);
            else
                return Curve.ShiftBackward(Leaving.RectTransform, Current.RectTransform);
        }
    }
}
