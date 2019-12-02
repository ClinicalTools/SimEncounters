namespace SimEncounters.Xml
{
    /// <summary>
    /// A class implementing this must implement a constructor with <seealso cref="XmlDeserializer"/> as its sole parameter.
    /// </summary>
    /// <remarks>
    /// While forcing a constructor is done by System.Runtime.Serialization, I strongly dislike forcing something the compiler won't pick up.
    /// A factory pattern would probably have been better and make it easier to override fewer classes.
    /// </remarks>
    public interface IXmlSerializable
    {
        void GetObjectData(XmlSerializer serializer);
    }
}