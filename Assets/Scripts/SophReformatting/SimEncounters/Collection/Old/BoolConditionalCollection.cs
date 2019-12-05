using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public class BoolConditionalCollection : ConditionalCollection<BoolConditional, bool>
    {
        protected const string VAL_NODE_NAME = "value";

        public override string CollectionNodeName => "boolConds";
        protected override string ValueNodeName => "boolCond";

        public BoolConditionalCollection() : base() { }
        /// <summary>
        /// Reads in a collection of bool conditionals from XML.
        /// </summary>
        /// <param name="caseVarsNode">Parent node of the conditional collection node.</param>
        public BoolConditionalCollection(XmlNode conditonalsNode) : base(conditonalsNode) { }

        protected override BoolConditional NewConditional(string varKey, XmlNode conditionalNode)
        {
            var valStr = conditionalNode[VAL_NODE_NAME]?.InnerText;
            if (bool.TryParse(valStr, out var val))
                return new BoolConditional(varKey, val);

            return null;
        }


        protected override void WriteConditionalElem(XmlElement valueElement, BoolConditional conditonal)
        {
            XmlHelper.CreateElement(VAL_NODE_NAME, conditonal.Value.ToString(), valueElement);
        }
    }
}