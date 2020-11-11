namespace ClinicalTools.SimEncounters.XmlSerialization
{
    public interface ISerializationFactory<T>
    {
        bool ShouldSerialize(T value);
        void Serialize(XmlSerializer serializer, T value);
        T Deserialize(XmlDeserializer deserializer);
    }
}