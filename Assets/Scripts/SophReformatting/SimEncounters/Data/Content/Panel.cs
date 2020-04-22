using ClinicalTools.SimEncounters.Collections;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public class Panel
    {
        // i'm really annoyed at  myself for not including the key until now. 
        // as it's in the ordered collection, it felt repetitive to include it
        // not having it here makes things more complicated as everything needs to refer to panel by the key
        public string Key { get; }
        public string Type { get; }
        public IDictionary<string, string> Data { get; } = new Dictionary<string, string>();
        public OrderedCollection<Panel> ChildPanels { get; } = new OrderedCollection<Panel>();
        public virtual ConditionalData Conditions { get; set; }
        public virtual PinData Pins { get; set; }

        public Panel(string type)
        {
            Type = type;
        }
        public Panel(string type, OrderedCollection<Panel> panels)
        {
            Type = type;
            ChildPanels = panels;
        }
    }
}