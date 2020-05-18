using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterPanelValueDisplay : IWriterPanelValueDisplay
    {
        public virtual BaseField[] Display(Encounter encounter, Panel panel, Transform transform)
        {
            var fields = transform.GetComponentsInChildren<BaseField>(true);
            var values = panel.Values;
            foreach (var field in fields) {
                var hasValue = values.ContainsKey(field.Name);
                string value = hasValue ? values[field.Name] : null;

                if (field is BaseValueField valueField) {
                    if (hasValue)
                        valueField.Initialize(value);
                    else
                        valueField.Initialize();
                } else if (field is BaseEncounterField encounterField) {
                    if (hasValue)
                        encounterField.Initialize(encounter, value);
                    else
                        encounterField.Initialize(encounter);
                }
            }

            return fields;
        }

        public virtual Dictionary<string, string> Serialize(IEnumerable<BaseField> fields)
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