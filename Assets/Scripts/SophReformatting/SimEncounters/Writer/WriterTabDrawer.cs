using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterTabDrawer : BaseTabDrawer
    {
        public BaseWriterPanelsCreator PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseWriterPanelsCreator panelCreator;

        public override void Display(Encounter encounter, Tab tab)
        {

        }
    }
}