using ClinicalTools.SimEncounters.Collections;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public class Panel
    {
        public string Type { get; }
        public IDictionary<string, string> Data { get; } = new Dictionary<string, string>();
        public OrderedCollection<Panel> ChildPanels { get; } = new OrderedCollection<Panel>();
        public virtual ConditionalData Conditions { get; set; }
        public virtual PinData Pins { get; set; }

        public Panel(string type)
        {
            Type = type;
        }
    }
}