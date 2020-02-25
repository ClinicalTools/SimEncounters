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
            if (tabUI is ReaderCheckboxesTabUI checkboxesTabUI)
                return new ReaderCheckboxesTabDisplay(Reader, checkboxesTabUI, keyedTab);
            else if (tabUI is ReaderQuizTabUI quizTabUI)
                return new ReaderQuizTabDisplay(Reader, quizTabUI, keyedTab);
            else if (tabUI is ReaderTabUI basicTabUI)
                return new ReaderTabDisplay(Reader, basicTabUI, keyedTab);
            else
                return default;
        }
    }
}