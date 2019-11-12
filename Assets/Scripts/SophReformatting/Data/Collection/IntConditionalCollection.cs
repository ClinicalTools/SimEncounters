using System.Xml;

namespace SimEncounters
{
    public class IntConditionalCollection : ConditionalCollection<IntConditional, int>
    {
        protected const string VAL_NODE_NAME = "value";
        protected const string COMPARATOR_NODE_NAME = "op";

        public override string CollectionNodeName => "intConds";
        protected override string ValueNodeName => "intCond";

        public IntConditionalCollection() : base() { }
        /// <summary>
        /// Reads in a collection of int conditionals from XML.
        /// </summary>
        /// <param name="caseVarsNode">Parent node of the conditional collection node.</param>
        public IntConditionalCollection(XmlNode conditionalsNode) : base(conditionalsNode) { }

        protected override IntConditional NewConditional(string varKey, XmlNode conditionalNode)
        {
            var valStr = conditionalNode[VAL_NODE_NAME]?.InnerText;
            var opStr = conditionalNode[COMPARATOR_NODE_NAME]?.InnerText;

            if (int.TryParse(valStr, out var val) && IntConditional.TryParseOperator(opStr, out var op))
                return new IntConditional(varKey, val, op);


            return null;
        }


        protected override void WriteConditionalElem(XmlElement valueElement, IntConditional conditonal)
        {
            XmlHelper.CreateElement(VAL_NODE_NAME, conditonal.Value.ToString(), valueElement);
            XmlHelper.CreateElement(COMPARATOR_NODE_NAME, conditonal.Comparator.ToString(), valueElement);
        }
    }
}