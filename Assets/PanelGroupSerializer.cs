using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Writer
{
    public class PanelGroupSerializer
    {
        public void Serialize(List<WriterPanelUI> writerPanels, OrderedCollection<Panel> panelCollection)
        {
            foreach (var writerPanel in writerPanels) {
                writerPanel.Serialize();
                if (writerPanel.Key == null)
                    writerPanel.Key = panelCollection.Add(writerPanel.Panel);
                writerPanel.UpdateIndex();
            }
            writerPanels.Sort();

            foreach (var writerPanel in writerPanels)
                panelCollection.MoveValue(writerPanel.Index, writerPanel.Panel);
        }
    }
}