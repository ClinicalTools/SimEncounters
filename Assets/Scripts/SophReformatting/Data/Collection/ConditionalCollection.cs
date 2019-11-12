using System.Xml;

namespace SimEncounters
{
    public abstract class ConditionalCollection<T, U> : SimCollection<T>
        where T : CaseConditional<U>
    {
        public ConditionalCollection() : base() { }
        /// <summary>
        /// Reads in a collection of conditionals from XML.
        /// </summary>
        /// <param name="caseVarsNode">Parent node of the conditional collection node.</param>
        public ConditionalCollection(XmlNode baseConditonalsNode) : base(baseConditonalsNode) { }

        protected override T ReadValueNode(XmlNode valueNode)
        {
            var varKey = valueNode["var"]?.InnerText;
            if (varKey == null)
                return null;

            return NewConditional(varKey, valueNode);
        }

        /// <summary>
        /// Creates a new variable with a given name and value as a string.
        /// </summary>
        /// <param name="varKey">Key of the variable the conditional checks</param>
        /// <param name="conditonalNode">Node representing the conditional.</param>
        /// <returns>The new conditional, or null if the value node is invalid.</returns>
        protected abstract T NewConditional(string varKey, XmlNode conditonalNode);

        protected override void WriteValueElem(XmlElement valueElement, T value)
        {
            XmlHelper.CreateElement("var", value.VarKey, valueElement);
            WriteConditionalElem(valueElement, value);
        }

        /// <summary>
        /// Writes nodes to a conditional element to match the value. 
        /// Variable key is written in a seperate method (WriteValueElem(XmlElement, T)).
        /// </summary>
        /// <param name="conditionalElement">Element to write values to</param>
        /// <param name="conditonal">Conditional to use values from</param>
        protected abstract void WriteConditionalElem(XmlElement conditionalElement, T conditonal);
    }
}