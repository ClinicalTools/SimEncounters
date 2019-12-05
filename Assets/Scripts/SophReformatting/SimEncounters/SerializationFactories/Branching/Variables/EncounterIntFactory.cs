using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class EncounterIntFactory : ISerializationFactory<EncounterInt>
    {
        protected virtual NodeInfo NameInfo { get; } = new NodeInfo("name");
        protected virtual NodeInfo ValueInfo { get; } = new NodeInfo("value");
        
        public virtual bool ShouldSerialize(EncounterInt value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, EncounterInt value)
        {
            serializer.AddString(NameInfo, value.Name);
            serializer.AddInt(ValueInfo, value.Value);
        }

        protected virtual string GetName(XmlDeserializer deserializer) => deserializer.GetString(NameInfo);
        protected virtual int GetValue(XmlDeserializer deserializer) => deserializer.GetInt(ValueInfo);
        public virtual EncounterInt Deserialize(XmlDeserializer deserializer)
        {
            var name = GetName(deserializer);
            var value = GetValue(deserializer);

            return new EncounterInt(name, value);
        }

    }
}