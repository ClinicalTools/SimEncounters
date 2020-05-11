using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Writer
{
    public class Something
    {
        public virtual BaseWriterPanel ChoosePrefab(List<BaseWriterPanel> panelOptions, Panel panel)
        {
            if (panelOptions.Count == 1)
                return panelOptions[0];

            foreach (var option in panelOptions) {
                if (string.Equals(option.Type, panel.Type, StringComparison.InvariantCultureIgnoreCase))
                    return option;
            }

            return panelOptions[0];
        }

        public virtual OrderedCollection<Panel> SerializeChildren(OrderedCollection<BaseWriterPanel> writerPanels)
        {
            var panelsArr = new KeyValuePair<string, BaseWriterPanel>[writerPanels.Count];
            foreach (var panel in writerPanels) {
                var index = panel.Value.transform.GetSiblingIndex();
                panelsArr[index] = panel;
            }

            var panels = new OrderedCollection<Panel>();
            foreach (var writerPanel in panelsArr) {
                var panel = writerPanel.Value.Serialize();
                if (writerPanel.Key == null)
                    panels.Add(panel);
                else
                    panels.Add(writerPanel.Key, panel);
            }
            return panels;
        }
    }
}