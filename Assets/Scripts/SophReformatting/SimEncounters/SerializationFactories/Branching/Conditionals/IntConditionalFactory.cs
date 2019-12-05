using System;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class IntConditionalFactory : ISerializationFactory<IntConditional>
    {
        protected virtual NodeInfo KeyInfo { get; } = new NodeInfo("var");
        protected virtual NodeInfo ValueInfo { get; } = new NodeInfo("value");
        protected virtual NodeInfo ComparatorInfo { get; } = new NodeInfo("op");

        public virtual bool ShouldSerialize(IntConditional value) => value != null;

        public void Serialize(XmlSerializer serializer, IntConditional value)
        {
            serializer.AddString(KeyInfo, value.VarKey);
            serializer.AddInt(KeyInfo, value.Value);
            serializer.AddString(ComparatorInfo, value.Comparator.ToString());
        }

        public IntConditional Deserialize(XmlDeserializer deserializer)
        {
            var key = GetVarKey(deserializer);
            var value = GetValue(deserializer);
            var comparator = GetComparator(deserializer);

            return new IntConditional(key, value, comparator);
        }

        protected virtual string GetVarKey(XmlDeserializer deserializer)
            => deserializer.GetString(KeyInfo);
        protected virtual int GetValue(XmlDeserializer deserializer)
            => deserializer.GetInt(ValueInfo);
        protected virtual IntComparator GetComparator(XmlDeserializer deserializer)
        {
            var comparatorString = deserializer.GetString(ComparatorInfo);
            return (IntComparator)Enum.Parse(typeof(IntComparator), comparatorString);
        }
    }
}