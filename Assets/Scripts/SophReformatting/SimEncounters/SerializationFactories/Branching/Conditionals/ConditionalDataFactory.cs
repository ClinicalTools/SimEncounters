using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class ConditionalDataFactory : ISerializationFactory<ConditionalData>
    {
        protected ISerializationFactory<BoolConditional> BoolConditionalFactory { get; }
        protected ISerializationFactory<IntConditional> IntConditionalFactory { get; }

        public ConditionalDataFactory(ISerializationFactory<BoolConditional> boolConditonalFactory, 
            ISerializationFactory<IntConditional> intConditonalFactory)
        {
            BoolConditionalFactory = boolConditonalFactory;
            IntConditionalFactory = intConditonalFactory;
        }

        protected virtual CollectionInfo BoolsInfo { get; } = new CollectionInfo("bools", "bool");
        protected virtual CollectionInfo IntsInfo { get; } = new CollectionInfo("ints", "int");


        public bool ShouldSerialize(ConditionalData value)
        {
            return value != null && (value.Bools.Count > 0 || value.Ints.Count > 0);
        }


        public void Serialize(XmlSerializer serializer, ConditionalData value)
        {
            serializer.AddKeyValuePairs(BoolsInfo, value.Bools, BoolConditionalFactory);
            serializer.AddKeyValuePairs(IntsInfo, value.Ints, IntConditionalFactory);
        }

        public ConditionalData Deserialize(XmlDeserializer deserializer)
        {
            var condData = CreateVariableData(deserializer);
            AddBoolConditionals(deserializer, condData);
            AddIntConditionals(deserializer, condData);

            return condData;
        }

        protected virtual ConditionalData CreateVariableData(XmlDeserializer deserializer) => new ConditionalData();


        protected virtual List<KeyValuePair<string, BoolConditional>> GetBoolConditionals(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(BoolsInfo, BoolConditionalFactory);
        protected virtual void AddBoolConditionals(XmlDeserializer deserializer, ConditionalData variableData)
        {
            var boolPairs = GetBoolConditionals(deserializer);
            if (boolPairs == null)
                return;

            foreach (var boolPair in boolPairs)
                variableData.Bools.Add(boolPair);
        }


        protected virtual List<KeyValuePair<string, IntConditional>> GetInts(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(IntsInfo, IntConditionalFactory);
        protected virtual void AddIntConditionals(XmlDeserializer deserializer, ConditionalData variableData)
        {
            var intPairs = GetInts(deserializer);
            if (intPairs == null)
                return;

            foreach (var intPair in intPairs)
                variableData.Ints.Add(intPair);
        }
    }
}