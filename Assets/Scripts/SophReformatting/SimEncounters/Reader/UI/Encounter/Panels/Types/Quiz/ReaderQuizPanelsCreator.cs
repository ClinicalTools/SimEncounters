using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderQuizPanelsCreator : BaseReaderPanelsCreator
    {
        public BaseReaderPanel MultipleChoicePanel { get => multipleChoicePanel; set => multipleChoicePanel = value; }
        [SerializeField] private BaseReaderPanel multipleChoicePanel;
        public BaseReaderPanel CheckBoxPanel { get => checkBoxPanel; set => checkBoxPanel = value; }
        [SerializeField] private BaseReaderPanel checkBoxPanel;

        protected BaseReaderPanel.Factory ReaderPanelFactory { get; set; }
        [Inject] public virtual void Inject(BaseReaderPanel.Factory readerPanelFactory) => ReaderPanelFactory = readerPanelFactory;

        public override List<BaseReaderPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels)
        {
            var panels = new List<BaseReaderPanel>();
            foreach (var childPanel in childPanels) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var panel = ReaderPanelFactory.Create(prefab);
                panel.transform.SetParent(transform);
                panel.transform.localScale = Vector3.one;
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