using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseWriterPanelsCreator : MonoBehaviour
    {
        public abstract List<BaseWriterPanel> DrawChildPanels(IEnumerable<Panel> childPanels);
    }

    public class WriterPanelsCreator : BaseWriterPanelsCreator
    {
        public override List<BaseWriterPanel> DrawChildPanels(IEnumerable<Panel> childPanels)
        {
            return null;
        }
    }

    public class WriterPresetPanelsCreator : BaseWriterPanelsCreator
    {
        [SerializeField] private BaseWriterPanel presetPanels;

        public override List<BaseWriterPanel> DrawChildPanels(IEnumerable<Panel> childPanels)
        {
            return null;
        }
    }
}