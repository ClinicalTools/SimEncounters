namespace SimEncounters.Xml
{
    public interface ISerializationFactory<T>
    {
        void Serialize(XmlSerializer serializer, T value);
        T Deserialize(XmlDeserializer deserializer);
    }
}