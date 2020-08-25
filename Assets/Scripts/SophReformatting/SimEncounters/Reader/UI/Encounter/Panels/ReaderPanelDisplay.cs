
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelDisplay : IReaderPanelDisplay
    {
        private readonly BaseUserPinGroupDrawer pinButtonsPrefab;
        public ReaderPanelDisplay(BaseUserPinGroupDrawer pinButtonsPrefab)
        {
            this.pinButtonsPrefab = pinButtonsPrefab;
        }

        public virtual void Display(UserPanel userPanel, Transform panelTransform, Transform pinParent)
        {
            CreatePinButtons(userPanel, pinParent);
            InitializePanelValueFields(userPanel, panelTransform);
        }

        protected virtual BaseUserPinGroupDrawer CreatePinButtons(UserPanel userPanel, Transform pinParent)
        {
            if (userPanel.PinGroup == null)
                return null;

            var pinButtons = Object.Instantiate(pinButtonsPrefab, pinParent);
            pinButtons.Display(userPanel.PinGroup);

            return pinButtons;
        }

        protected virtual BaseField[] InitializePanelValueFields(UserPanel userPanel, Transform transform)
        {
            var fields = transform.GetComponentsInChildren<BaseField>(true);
            var values = userPanel.Data.Values;
            var encounter = userPanel.Encounter.Data;
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
                } else if (field is BaseUserPanelField userPanelField) {
                    if (hasValue)
                        userPanelField.Initialize(userPanel, value);
                    else
                        userPanelField.Initialize(userPanel);
                }
            }

            return fields;
        }
    }
}