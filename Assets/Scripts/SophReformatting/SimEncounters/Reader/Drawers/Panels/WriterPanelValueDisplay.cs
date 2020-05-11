using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterPanelValueDisplay : IWriterPanelValueDisplay
    {
        public virtual IField[] Display(Encounter encounter, Panel panel, Transform transform)
        {
            var fields = transform.GetComponentsInChildren<IField>(true);
            foreach (var field in fields) {
                if (field is IValueField valueField ) {
                    if (panel.Values.ContainsKey(field.Name))
                        valueField.Initialize(panel.Values[field.Name]);
                    else
                        valueField.Initialize();
                } else if (field is IEncounterField encounterField) {
                    if (panel.Values.ContainsKey(field.Name))
                        encounterField.Initialize(encounter, panel.Values[field.Name]);
                    else
                        encounterField.Initialize(encounter);
                }
            }

            return fields;
        }

        public virtual Dictionary<string, string> Serialize(IEnumerable<IField> fields)
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