using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface IReaderTabDisplay
    {
        void Display(KeyValuePair<string, Tab> keyedTab);
        void Destroy();
    }
    public interface IReaderPanelDisplay
    {
        void Display(KeyValuePair<string, Panel> keyedPanel);
    }
}