using System.Xml;

namespace SimEncounters
{
    /// <summary>
    /// Handles the reading of a value from xml and writing a value to xml.
    /// </summary>
    /// <typeparam name="T">Type of the element being read/written</typeparam>
    /// <remarks>
    /// XmlSerializer exists but is rather gross in encouraging public fields.
    /// However, the primary reason for handling the XML by hand is because
    /// it was previously done by hand, so at the least, a legacy interpretter is needed.
    /// Doing it through custom claasses has the added benefit of allowing the formatting 
    /// to evlove, while still allowing legacy data to be interpretted.
    /// </remarks>
    public abstract class XmlSerializer<T>
    {
        public abstract string NodeName { get; }
        public virtual bool ValidNodeName(string name) => NodeName == name;

        public virtual XmlNode GetValueNode(XmlNode parentNode) => parentNode[NodeName];

        public virtual T DeserializeFromParent(XmlNode parentNode)
        {
            var valueNode = GetValueNode(parentNode);
            return Deserialize(valueNode);
        }
        public abstract T Deserialize(XmlNode valueNode);
        public virtual XmlNode Serialize(T value)
        {
            var valueElement = XmlHelper.CreateElement(NodeName);
            SerializeValues(valueElement, value);
            return valueElement;
        }

        /// <summary>
        /// Modifies an XmlElement to represent the passed value.
        /// </summary>
        /// <param name="valueElement"></param>
        /// <param name="value">Value to create an XmlElement represented</param>
        /// <returns>The modified XmlElement</returns>
        protected abstract void SerializeValues(XmlElement valueElement, T value);

    }
}