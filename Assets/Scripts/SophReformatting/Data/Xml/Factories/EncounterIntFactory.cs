namespace SimEncounters.Xml
{
    public class EncounterIntFactory : ISerializationFactory<EncounterInt>
    {
        protected virtual NodeInfo NameName { get; } = new NodeInfo("name");
        protected virtual NodeInfo ValueName { get; } = new NodeInfo("value");

        public virtual void Serialize(XmlSerializer serializer, EncounterInt value)
        {
            serializer.AddString(NameName, value.Name);
            serializer.AddInt(ValueName, value.Value);
        }

        protected virtual string GetName(XmlDeserializer deserializer) => deserializer.GetString(NameName);
        protected virtual int GetValue(XmlDeserializer deserializer) => deserializer.GetInt(ValueName);
        public virtual EncounterInt Deserialize(XmlDeserializer deserializer)
        {
            var name = GetName(deserializer);
            var value = GetValue(deserializer);

            return new EncounterInt(name, value);
        }

    }
}