using SimEncounters.Data;
using System;
using UnityEngine;

namespace SimEncounters.Xml
{
    public class IconFactory : ISerializationFactory<Icon>
    {
        protected virtual NodeInfo ColorName { get; } = new NodeInfo("color");
        protected virtual NodeInfo ReferenceName { get; } = new NodeInfo("reference");

        public void Serialize(XmlSerializer serializer, Icon icon)
        {
            serializer.AddColor(ColorName, icon.Color);
            serializer.AddString(ReferenceName, icon.Reference);
        }

        protected virtual Color GetColor(XmlDeserializer deserializer) => deserializer.GetColor(ColorName);
        protected virtual string GetReference(XmlDeserializer deserializer) => deserializer.GetString(ReferenceName);
        public Icon Deserialize(XmlDeserializer deserializer)
        {
            var reference = GetReference(deserializer);
            if (string.IsNullOrWhiteSpace(reference))
                throw new Exception("Invalid icon reference.");
            var color = GetColor(deserializer);

            return new Icon(color, reference);
        }
    }
}