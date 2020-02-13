using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderValueFieldInitializer
    {
        protected EncounterReader Reader { get; }
        public ReaderValueFieldInitializer(EncounterReader reader)
        {
            Reader = reader;
        }

        public virtual IValueField[] InitializePanelValueFields(GameObject panelObject, Panel panel)
        {
            var valueFields = panelObject.GetComponentsInChildren<IValueField>(true);
            foreach (var valueField in valueFields) {
                if (panel.Data.ContainsKey(valueField.Name))
                    valueField.Initialize(panel.Data[valueField.Name]);
                else
                    valueField.Initialize();
            }
            var readerValueFields = panelObject.GetComponentsInChildren<IReaderValueField>(true);
            foreach (var valueField in readerValueFields) {
                if (panel.Data.ContainsKey(valueField.Name))
                    valueField.Initialize(Reader, panel.Data[valueField.Name]);
                else
                    valueField.Initialize(Reader);
            }

            return valueFields;
        }
    }
}