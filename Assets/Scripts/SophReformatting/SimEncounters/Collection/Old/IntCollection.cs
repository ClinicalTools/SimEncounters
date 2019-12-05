using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public class IntCollection : EncounterVarCollection<EncounterInt, int>
    {
        public override string CollectionNodeName => "ints";
        protected override string ValueNodeName => "int";

        public IntCollection() : base() { }
        /// <summary>
        /// Reads in a collection of encounter ints from XML.
        /// </summary>
        /// <param name="encounterVarsNode">Parent node of the int collection node.</param>
        public IntCollection(XmlNode encounterVarsNode) : base(encounterVarsNode) { }

        protected override EncounterInt NewEncounterVar(string name, string valueStr)
        {
            if (int.TryParse(valueStr, out var val))
                return new EncounterInt(name, val);

            return null;
        }
    }
}