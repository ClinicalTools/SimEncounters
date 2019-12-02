using System;

namespace SimEncounters.Xml
{
    public class IntConditionalFactory : ISerializationFactory<IntConditional>
    {
        protected virtual NodeInfo KeyName { get; } = new NodeInfo("var");
        protected virtual NodeInfo ValueName { get; } = new NodeInfo("value");
        protected virtual NodeInfo ComparatorName { get; } = new NodeInfo("op");

        public void Serialize(XmlSerializer serializer, IntConditional value)
        {
            serializer.AddString(KeyName, value.VarKey);
            serializer.AddInt(KeyName, value.Value);
            serializer.AddString(ComparatorName, value.Comparator.ToString());
        }

        public IntConditional Deserialize(XmlDeserializer deserializer)
        {
            var key = GetVarKey(deserializer);
            var value = GetValue(deserializer);
            var comparator = GetComparator(deserializer);

            return new IntConditional(key, value, comparator);
        }

        protected virtual string GetVarKey(XmlDeserializer deserializer)
            => deserializer.GetString(KeyName);
        protected virtual int GetValue(XmlDeserializer deserializer)
            => deserializer.GetInt(ValueName);
        protected virtual IntComparator GetComparator(XmlDeserializer deserializer)
        {
            var comparatorString = deserializer.GetString(ComparatorName);
            return (IntComparator)Enum.Parse(typeof(IntComparator), comparatorString);
        }
    }
}