using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class WriterPanelValueDisplay : IWriterPanelValueDisplay
    {
        public virtual IPanelField[] Display(Encounter encounter, Panel panel, Transform transform)
        {
            var fields = transform.GetComponentsInChildren<IPanelField>(true);
            foreach (var field in fields)
                InitializeField(encounter, panel.Values, field);

            return fields;
        }

        protected virtual void InitializeField(Encounter encounter, IDictionary<string, string> values, IPanelField field)
        {
            var hasValue = values.ContainsKey(field.Name);
            string value = hasValue ? values[field.Name] : null;

            if (field is IValuePanelField valueField) {
                if (hasValue)
                    valueField.Initialize(value);
                else
                    valueField.Initialize();
            } else if (field is IEncounterPanelField encounterField) {
                if (hasValue)
                    encounterField.Initialize(encounter, value);
                else
                    encounterField.Initialize(encounter);
            }
        }

        public virtual Dictionary<string, string> Serialize(IEnumerable<IPanelField> fields)
        {
            var values = new Dictionary<string, string>();
            foreach (var valueField in fields) {
                if (valueField.Value == null)
                    continue;
                if (values.ContainsKey(valueField.Name))
                    Debug.LogError($"Duplicate panel value field name ({valueField.Name})");
                else
                    values.Add(valueField.Name, valueField.Value);
            }

            return values;
        }
    }
}