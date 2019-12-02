namespace SimEncounters.Xml
{
    public class EncounterBoolFactory : ISerializationFactory<EncounterBool>
    {
        protected virtual NodeInfo NameName { get; } = new NodeInfo("name");
        protected virtual NodeInfo ValueName { get; } = new NodeInfo("value");

        public virtual void Serialize(XmlSerializer serializer, EncounterBool value)
        {
            serializer.AddString(NameName, value.Name);
            serializer.AddBool(ValueName, value.Value);
        }

        protected virtual string GetName(XmlDeserializer deserializer) => deserializer.GetString(NameName);
        protected virtual bool GetValue(XmlDeserializer deserializer) => deserializer.GetBool(ValueName);
        public virtual EncounterBool Deserialize(XmlDeserializer deserializer)
        {
            var name = GetName(deserializer);
            var value = GetValue(deserializer);

            return new EncounterBool(name, value);
        }

    }
}