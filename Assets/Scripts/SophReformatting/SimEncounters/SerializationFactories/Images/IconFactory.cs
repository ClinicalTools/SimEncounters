using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class IconFactory : ISerializationFactory<Icon>
    {
        protected virtual NodeInfo ColorInfo { get; } = new NodeInfo("color");
        protected virtual NodeInfo ReferenceInfo { get; } = new NodeInfo("reference");

        public virtual bool ShouldSerialize(Icon value) => value != null;

        public void Serialize(XmlSerializer serializer, Icon icon)
        {
            // Icon color is never actually used for anything beyond Clinical Encounter legacy support, so it isn't serialized
            // Icons will remain their own class to allow easier support for custom icons if that ever becomes necessary in the future
            serializer.AddString(ReferenceInfo, icon.Reference);
        }

        protected virtual Color GetColor(XmlDeserializer deserializer) => deserializer.GetColor(ColorInfo);
        protected virtual string GetReference(XmlDeserializer deserializer) => deserializer.GetString(ReferenceInfo);
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