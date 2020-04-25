using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizPanelsDrawer : BaseChildPanelsDrawer
    {
        [SerializeField] private BaseReaderPanelUI multipleChoicePanel;
        public BaseReaderPanelUI MultipleChoicePanel { get => multipleChoicePanel; set => multipleChoicePanel = value; }
        [SerializeField] private BaseReaderPanelUI checkBoxPanel;
        public BaseReaderPanelUI CheckBoxPanel { get => checkBoxPanel; set => checkBoxPanel = value; }

        public override List<BaseReaderPanelUI> DrawChildPanels(IEnumerable<UserPanel> childPanels)
        {
            var panels = new List<BaseReaderPanelUI>();
            foreach (var childPanel in childPanels) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var panel = Instantiate(prefab, transform);
                panel.Display(childPanel);
                panels.Add(panel);
            }

            return panels;
        }

        protected virtual BaseReaderPanelUI GetChildPanelPrefab(UserPanel panel)
        {
            if (panel.Data.Data.ContainsKey("OptionTypeValue") && panel.Data.Data["OptionTypeValue"] == "Multiple Choice")
                return MultipleChoicePanel;
            else
                return CheckBoxPanel;
        }
    }
}