using System.Xml;

namespace SimEncounters
{
    public abstract class EncounterVarCollection<T, U> : SimCollection<T>
        where T : EncounterVariable<U>
    {
        public EncounterVarCollection() : base() { }
        /// <summary>
        /// Reads in a collection of encounter vars from XML.
        /// </summary>
        /// <param name="encounterVarsParentNode">Parent node of the encounter vars node.</param>
        public EncounterVarCollection(XmlNode encounterVarsParentNode) : base(encounterVarsParentNode) { }

        protected override T ReadValueNode(XmlNode valueNode)
        {
            var name = valueNode["name"]?.InnerText;
            var valStr = valueNode["value"]?.InnerText;
            if (name == null || valStr == null)
                return null;

            return NewEncounterVar(name, valStr);
        }

        /// <summary>
        /// Creates a new variable with a given name and value as a string.
        /// </summary>
        /// <param name="name">Name of the case variable</param>
        /// <param name="valueStr">A string representation of the value stored in the case variable</param>
        /// <returns>The new case variable, or null if the value string is invalid.</returns>
        protected abstract T NewEncounterVar(string name, string valueStr);

        protected override void WriteValueElem(XmlElement valueElement, T value)
        {
            XmlHelper.CreateElement("name", value.Name, valueElement);
            XmlHelper.CreateElement("value", GetValueString(value.Value), valueElement);
        }

        /// <summary>
        /// Gets the string representation of the value of the case variable.
        /// </summary>
        /// <param name="value">Value of the case variable</param>
        /// <returns>The ToString() of the value</returns>
        protected virtual string GetValueString(U value)
        {
            return value.ToString();
        }
    }
}