using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class EncounterBoolFactory : ISerializationFactory<EncounterBool>
    {
        protected virtual NodeInfo NameInfo { get; } = new NodeInfo("name");
        protected virtual NodeInfo ValueInfo { get; } = new NodeInfo("value");

        public virtual bool ShouldSerialize(EncounterBool value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, EncounterBool value)
        {
            serializer.AddString(NameInfo, value.Name);
            serializer.AddBool(ValueInfo, value.Value);
        }

        protected virtual string GetName(XmlDeserializer deserializer) => deserializer.GetString(NameInfo);
        protected virtual bool GetValue(XmlDeserializer deserializer) => deserializer.GetBool(ValueInfo);
        public virtual EncounterBool Deserialize(XmlDeserializer deserializer)
        {
            var name = GetName(deserializer);
            var value = GetValue(deserializer);

            return new EncounterBool(name, value);
        }

    }
}