using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelUI : BaseReaderPanelUI
    {
        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private Transform childrenParent;
        public virtual Transform ChildrenParent { get => childrenParent; set => childrenParent = value; }

        [SerializeField] private List<ReaderPanelUI> childPanelOptions;
        public virtual List<ReaderPanelUI> ChildPanelOptions { get => childPanelOptions; set => childPanelOptions = value; }


        protected UserPinGroupDrawer PinButtons { get; set; }
        protected IPanelFieldInitializer FieldInitializer { get; set; }
        
        [Inject]
        public virtual void Inject(UserPinGroupDrawer pinButtons, IPanelFieldInitializer fieldInitializer)
        {
            PinButtons = pinButtons;
            FieldInitializer = fieldInitializer;
        }

        public override void Display(UserPanel userPanel)
        {
            CreatePinButtons(userPanel); 
            FieldInitializer.InitializePanelValueFields(transform, userPanel);
            DeserializeChildren(userPanel.GetChildPanels());
        }

        protected virtual void DeserializeChildren(IEnumerable<UserPanel> panels)
        {
            foreach (var userPanel in panels)
                DeserializeChild(userPanel);
        }
        protected virtual UserPanelDrawer DeserializeChild(UserPanel userPanel)
        {
            var selector = new ReaderPanelDrawerSelector();
            var panelPrefab = selector.GetPanelPrefab(userPanel.Data.Type, ChildPanelOptions);
            var panelUI = Instantiate(panelPrefab, ChildrenParent);
            panelUI.Display(userPanel);
            return panelUI;
        }

        protected virtual UserPinGroupDrawer CreatePinButtons(UserPanel userPanel) {
            if (userPanel.PinGroup == null)
                return null;

            var pinButtons = Instantiate(PinButtons, transform);
            pinButtons.Display(userPanel.PinGroup);

            return pinButtons;
        }
    }

    public abstract class UserPinGroupDrawer : MonoBehaviour
    {
        public abstract void Display(UserPinGroup userPanel);
    }
    public interface IPanelFieldInitializer
    {
        IValueField[] InitializePanelValueFields(Transform panelTransform, UserPanel userPanel);
    }

    public class PanelFieldInitializer : IPanelFieldInitializer
    {
        public virtual IValueField[] InitializePanelValueFields(Transform panelTransform, UserPanel userPanel)
        {
            var valueFields = panelTransform.GetComponentsInChildren<IValueField>(true);
            foreach (var valueField in valueFields) {
                if (userPanel.Data.Data.ContainsKey(valueField.Name))
                    valueField.Initialize(userPanel.Data.Data[valueField.Name]);
                else
                    valueField.Initialize();
            }
            var readerValueFields = panelTransform.GetComponentsInChildren<IUserValueField>(true);
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