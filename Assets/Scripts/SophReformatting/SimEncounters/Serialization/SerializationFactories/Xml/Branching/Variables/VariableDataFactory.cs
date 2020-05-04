using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class VariableDataFactory : ISerializationFactory<VariableData>
    {
        protected virtual ISerializationFactory<EncounterBool> BoolFactory { get; }
        protected virtual ISerializationFactory<EncounterInt> IntFactory { get; } = new EncounterIntFactory();
        public VariableDataFactory(ISerializationFactory<EncounterBool> boolFactory, ISerializationFactory<EncounterInt> intFactory)
        {
            BoolFactory = boolFactory;
            IntFactory = intFactory;
        }

        protected virtual CollectionInfo BoolsInfo { get; } = new CollectionInfo("bools", "bool");
        protected virtual CollectionInfo IntsInfo { get; } = new CollectionInfo("ints", "int");


        public bool ShouldSerialize(VariableData value)
        {
            return value != null && (value.Bools.Count > 0 || value.Ints.Count > 0);
        }


        public void Serialize(XmlSerializer serializer, VariableData value)
        {
            serializer.AddKeyValuePairs(BoolsInfo, value.Bools, BoolFactory);
            serializer.AddKeyValuePairs(IntsInfo, value.Ints, IntFactory);
        }

        public VariableData Deserialize(XmlDeserializer deserializer)
        {
            var varData = CreateVariableData(deserializer);
            AddBools(deserializer, varData);
            AddInts(deserializer, varData);

            return varData;
        }

        protected virtual VariableData CreateVariableData(XmlDeserializer deserializer) => new VariableData();


        protected virtual List<KeyValuePair<string, EncounterBool>> GetBools(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(BoolsInfo, BoolFactory);
        protected virtual void AddBools(XmlDeserializer deserializer, VariableData variableData)
        {
            var boolPairs = GetBools(deserializer);
            if (boolPairs == null)
                return;

            foreach (var boolPair in boolPairs)
                variableData.Bools.Add(boolPair);
        }


        protected virtual List<KeyValuePair<string, EncounterInt>> GetInts(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(IntsInfo, IntFactory);
        protected virtual void AddInts(XmlDeserializer deserializer, VariableData variableData)
        {
            var intPairs = GetInts(deserializer);
            if (intPairs == null)
                return;

            foreach (var intPair in intPairs)
                variableData.Ints.Add(intPair);
        }
    }
}