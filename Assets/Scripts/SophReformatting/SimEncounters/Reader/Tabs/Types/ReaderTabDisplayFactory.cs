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

        public IReaderTabDisplay CreateTab(IReaderTabUI tabUI)
        {
            if (tabUI is ReaderCheckboxesTabUI checkboxesTabUI)
                return new ReaderCheckboxesTabDisplay(Reader, checkboxesTabUI);
            else if (tabUI is ReaderQuizTabUI quizTabUI)
                return new ReaderQuizTabDisplay(Reader, quizTabUI);
            else if (tabUI is ReaderTabUI basicTabUI)
                return new ReaderTabDisplay(Reader, basicTabUI);
            else
                return default;
        }
    }
}