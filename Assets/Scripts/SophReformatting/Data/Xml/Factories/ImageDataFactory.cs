using SimEncounters.Data;
using System;

namespace SimEncounters.Xml
{
    public class ImageDataFactory : ISerializationFactory<ImageData>
    {
        public void Serialize(XmlSerializer serializer, ImageData value)
        {
            //serializer.AddKeyValuePairs("", "", value.Icons);
            throw new NotImplementedException();
        }

        public ImageData Deserialize(XmlDeserializer deserializer)
        {

            throw new NotImplementedException();
        }
    }
}
