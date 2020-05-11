﻿using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseWriterPanelsDrawer : MonoBehaviour
    {
        public abstract List<BaseWriterPanel> DrawChildPanels(Encounter encounter, OrderedCollection<Panel> childPanels);
        public abstract List<BaseWriterPanel> DrawDefaultChildPanels(Encounter encounter);
        public abstract OrderedCollection<Panel> SerializeChildren();
    }
}