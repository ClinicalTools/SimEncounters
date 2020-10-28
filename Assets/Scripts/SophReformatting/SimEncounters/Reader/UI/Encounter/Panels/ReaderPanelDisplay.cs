using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelDisplay : IReaderPanelDisplay
    {
        public virtual void Display(UserPanel userPanel, Transform panelTransform, Transform pinParent)
        {
            CreatePinButtons(userPanel, pinParent);
            InitializePanelValueFields(userPanel, panelTransform);
        }

        protected BaseUserPinGroupDrawer PinButtons { get; set; }
        protected virtual BaseUserPinGroupDrawer CreatePinButtons(UserPanel userPanel, Transform pinParent)
        {
            if (userPanel.PinGroup == null)
                return null;

            return null;
        }

        protected virtual IPanelField[] InitializePanelValueFields(UserPanel userPanel, Transform transform)
        {
            var fields = transform.GetComponentsInChildren<IPanelField>(true);
            var values = userPanel.Data.Values;
            var encounter = userPanel.Encounter.Data;
            foreach (var field in fields) {
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
                } else if (field is BaseUserPanelField userPanelField) {
                    if (hasValue)
                        userPanelField.Initialize(userPanel, value);
                    else
                        userPanelField.Initialize(userPanel);
                }
            }

            return fields;
        }

        public virtual void Dispose()
        {
            if (PinButtons == null)
                return;
            //PinButtonsPool.Despawn(PinButtons);
            PinButtons = null;
        }
    }
}