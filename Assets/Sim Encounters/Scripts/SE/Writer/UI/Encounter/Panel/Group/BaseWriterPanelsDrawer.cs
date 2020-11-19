using ClinicalTools.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseWriterPanelsDrawer : MonoBehaviour
    {
        public abstract List<BaseWriterPanel> DrawChildPanels(Encounter encounter, OrderedCollection<Panel> childPanels);
        public abstract List<BaseWriterPanel> DrawDefaultChildPanels(Encounter encounter);
        public abstract OrderedCollection<Panel> SerializeChildren();
    }
}