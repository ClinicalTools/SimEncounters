using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderFooter
    {
        public event Action NextSection;
        public event Action NextTab;
        public event Action FinishCase;

        protected virtual OrderedCollection<Section> Sections { get; }
        protected virtual ReaderFooterUI FooterUI { get; }
        protected int PageCount { get; }
        protected virtual string GetPageInfoText(int pageNumber, int pageCount) => $"Page: {pageNumber}/{pageCount}";

        public ReaderFooter(ReaderScene reader, ReaderFooterUI footerUI, OrderedCollection<Section> sections)
        {
            Sections = sections;
            FooterUI = footerUI;
            PageCount = TabCount(sections.Values);
            footerUI.NextSectionButton.onClick.AddListener(() => NextSection?.Invoke());
            footerUI.NextTabButton.onClick.AddListener(() => NextTab?.Invoke());
            footerUI.FinishCaseButton.onClick.AddListener(() => FinishCase?.Invoke());
        }

        public void SetTab(Section currentSection)
        {
            var currentPage = GetCurrentPage(Sections.Values, currentSection);
            FooterUI.PageInfoLabel.text = GetPageInfoText(currentPage, PageCount);
            var hasNextTab = currentSection.CurrentTabIndex < currentSection.Tabs.Count - 1;
            var isLastSection = Sections.IndexOf(currentSection) == Sections.Count - 1;
            FooterUI.NextTabButton.gameObject.SetActive(hasNextTab);
            FooterUI.NextSectionButton.gameObject.SetActive(!hasNextTab && !isLastSection);
            FooterUI.FinishCaseButton.gameObject.SetActive(!hasNextTab && isLastSection);
        }

        protected virtual int GetCurrentPage(IEnumerable<Section> sections, Section currentSection)
        {
            // page is 1-indexed
            var count = 1;
            foreach (var section in sections) {
                if (section == currentSection)
                    return count + currentSection.CurrentTabIndex;
                count += section.Tabs.Count;
            }
            return -1;
        }
        
        protected virtual int TabCount(IEnumerable<Section> sections)
        {
            var count = 0;
            foreach (var section in sections)
                count += section.Tabs.Count;
            return count;
        }
    }
}