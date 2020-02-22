using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabDisplayFactory
    {
        protected ReaderScene Reader { get; }
        public ReaderTabDisplayFactory(ReaderScene reader) {
            Reader = reader;
        }

        public IReaderTabDisplay CreateTab(IReaderTabUI tabUI, KeyValuePair<string, Tab> keyedTab)
        {
            if (tabUI is ReaderCheckboxesTabUI)
                return new ReaderCheckboxesTabDisplay(Reader, (ReaderCheckboxesTabUI)tabUI, keyedTab);
            else if (tabUI is ReaderQuizTabUI)
                return new ReaderQuizTabDisplay(Reader, (ReaderQuizTabUI)tabUI, keyedTab);
            else if (tabUI is ReaderTabUI)
                return new ReaderTabDisplay(Reader, (ReaderTabUI)tabUI, keyedTab);
            else
                return default;
        }
    }
}