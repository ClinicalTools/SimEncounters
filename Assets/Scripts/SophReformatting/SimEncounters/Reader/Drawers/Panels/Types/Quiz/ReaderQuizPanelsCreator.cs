using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderQuizPanelsCreator : BaseReaderPanelsCreator
    {
        [SerializeField] private BaseReaderPanel multipleChoicePanel;
        public BaseReaderPanel MultipleChoicePanel { get => multipleChoicePanel; set => multipleChoicePanel = value; }
        [SerializeField] private BaseReaderPanel checkBoxPanel;
        public BaseReaderPanel CheckBoxPanel { get => checkBoxPanel; set => checkBoxPanel = value; }

        public override List<BaseReaderPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels)
        {
            var panels = new List<BaseReaderPanel>();
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

        protected virtual BaseReaderPanel GetChildPanelPrefab(UserPanel panel)
        {
            if (panel.Data.Values.ContainsKey("OptionTypeValue") && panel.Data.Values["OptionTypeValue"] == "Multiple Choice")
                return MultipleChoicePanel;
            else
                return CheckBoxPanel;
        }
    }
}