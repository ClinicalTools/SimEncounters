using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class BasicReaderPanelDrawer
    {
        private readonly UserPinGroupDrawer pinButtonsPrefab;
        public BasicReaderPanelDrawer(UserPinGroupDrawer pinButtonsPrefab)
        {
            this.pinButtonsPrefab = pinButtonsPrefab;
        }

        public void Display(UserPanel userPanel, Transform panelTransform, Transform pinParent)
        {
            CreatePinButtons(userPanel, pinParent);
            InitializePanelValueFields(userPanel, panelTransform);
        }

        protected virtual UserPinGroupDrawer CreatePinButtons(UserPanel userPanel, Transform pinParent)
        {
            if (userPanel.PinGroup == null)
                return null;

            var pinButtons = Object.Instantiate(pinButtonsPrefab, pinParent);
            pinButtons.Display(userPanel.PinGroup);

            return pinButtons;
        }

        protected virtual IValueField[] InitializePanelValueFields(UserPanel userPanel, Transform transform)
        {
            var valueFields = transform.GetComponentsInChildren<IValueField>(true);
            foreach (var valueField in valueFields) {
                if (userPanel.Data.Data.ContainsKey(valueField.Name))
                    valueField.Initialize(userPanel.Data.Data[valueField.Name]);
                else
                    valueField.Initialize();
            }
            var readerValueFields = transform.GetComponentsInChildren<IUserValueField>(true);
            foreach (var valueField in readerValueFields) {
                if (userPanel.Data.Data.ContainsKey(valueField.Name))
                    valueField.Initialize(userPanel, userPanel.Data.Data[valueField.Name]);
                else
                    valueField.Initialize(userPanel);
            }

            return valueFields;
        }
    }
}