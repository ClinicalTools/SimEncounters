using ClinicalTools.SimEncounters.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabButton : EncounterButton<Tab>
    {
        public TabButton(TabButtonUI tabButtonUI, Tab tab, EncounterWriter writer) : base(tabButtonUI.SelectButton, tab)
        {
            tabButtonUI.NameLabel.text = tab.Name;
        }
    }
}