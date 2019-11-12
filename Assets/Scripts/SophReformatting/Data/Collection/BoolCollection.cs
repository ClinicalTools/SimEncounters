using System.Xml;

namespace SimEncounters
{
    public class BoolCollection : EncounterVarCollection<EncounterBool, bool>
    {
        public override string CollectionNodeName => "bools";
        protected override string ValueNodeName => "bool";

        public BoolCollection() : base() { }

        /// <summary>
        /// Reads in a collection of encounter bools from XML.
        /// </summary>
        /// <param name="encounterVarsNode">Parent node of the bool collection node.</param>
        public BoolCollection(XmlNode encounterVarsNode) : base(encounterVarsNode) { }

        protected override EncounterBool NewEncounterVar(string name, string valueStr)
        {
            if (bool.TryParse(valueStr, out var val))
                return new EncounterBool(name, val);

            return null;
        }
    }
}