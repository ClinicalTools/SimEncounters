namespace SimEncounters.Xml
{
    /// <summary>
    /// A class implementing this must implement a constructor with <seealso cref="XmlDeserializer"/> as its sole parameter.
    /// </summary>
    public interface IXmlSerializable
    {
        void GetObjectData(XmlSerializer serializer);
    }
}