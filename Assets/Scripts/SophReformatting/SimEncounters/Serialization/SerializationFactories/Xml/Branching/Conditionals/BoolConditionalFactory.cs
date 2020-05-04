using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class BoolConditionalFactory : ISerializationFactory<BoolConditional>
    {
        protected virtual NodeInfo KeyInfo { get; } = new NodeInfo("var");
        protected virtual NodeInfo ValueInfo { get; } = new NodeInfo("value");

        public virtual bool ShouldSerialize(BoolConditional value) => value != null;

        public virtual void Serialize(XmlSerializer serializer, BoolConditional value)
        {
            serializer.AddString(KeyInfo, value.VarKey);
            serializer.AddBool(KeyInfo, value.Value);
        }

        protected virtual string GetVarKey(XmlDeserializer deserializer)
            => deserializer.GetString(KeyInfo);
        protected virtual bool GetValue(XmlDeserializer deserializer)
            => deserializer.GetBool(ValueInfo);
        public virtual BoolConditional Deserialize(XmlDeserializer deserializer)
        {
            var key = GetVarKey(deserializer);
            var value = GetValue(deserializer);

            return new BoolConditional(key, value);
        }
    }
}