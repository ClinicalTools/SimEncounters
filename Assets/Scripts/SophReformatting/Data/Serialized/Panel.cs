using System.Collections.Generic;

namespace SimEncounters.Data
{
    public class Panel
    {
        public string Type { get; }
        public IDictionary<string, string> Data { get; } = new Dictionary<string, string>();
        public OrderedDictionary<Panel> ChildPanels { get; } = new OrderedDictionary<Panel>();
        public virtual List<string> Conditions { get; set; }

        public Panel(string type)
        {
            Type = type;
        }
    }
}