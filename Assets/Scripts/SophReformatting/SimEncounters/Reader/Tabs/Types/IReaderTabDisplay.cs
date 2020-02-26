using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface IReaderTabDisplay
    {
        void Destroy();
    }
    public interface IReaderPanelDisplay
    {
        void Display(KeyValuePair<string, Panel> keyedPanel);
    }
}