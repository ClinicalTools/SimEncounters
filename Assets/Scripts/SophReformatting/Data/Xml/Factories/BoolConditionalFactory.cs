namespace SimEncounters.Xml
{
    public class BoolConditionalFactory : ISerializationFactory<BoolConditional>
    {
        protected virtual NodeInfo KeyName { get; } = new NodeInfo("var");
        protected virtual NodeInfo ValueName { get; } = new NodeInfo("value");

        public void Serialize(XmlSerializer serializer, BoolConditional value)
        {
            serializer.AddString(KeyName, value.VarKey);
            serializer.AddBool(KeyName, value.Value);
        }

        protected virtual string GetVarKey(XmlDeserializer deserializer)
            => deserializer.GetString(KeyName);
        protected virtual bool GetValue(XmlDeserializer deserializer)
            => deserializer.GetBool(ValueName);
        public BoolConditional Deserialize(XmlDeserializer deserializer)
        {
            var key = GetVarKey(deserializer);
            var value = GetValue(deserializer);

            return new BoolConditional(key, value);
        }
    }
}