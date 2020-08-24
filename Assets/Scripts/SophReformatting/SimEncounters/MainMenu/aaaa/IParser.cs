using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public interface IParser<T>
    {
        T Parse(string text);
    }

    public class XmlDeserializerParser<T> : IParser<T>
    {
        private readonly IParser<XmlDocument> xmlParser;
        private readonly ISerializationFactory<T> serializationFactory;
        public XmlDeserializerParser(IParser<XmlDocument> xmlParser, ISerializationFactory<T> serializationFactory)
        {
            this.xmlParser = xmlParser;
            this.serializationFactory = serializationFactory;
        }

        public T Parse(string text)
        {
            var xmlDoc = xmlParser.Parse(text);
            var deserializer = new XmlDeserializer(xmlDoc);
            return serializationFactory.Deserialize(deserializer);
        }
    }
}