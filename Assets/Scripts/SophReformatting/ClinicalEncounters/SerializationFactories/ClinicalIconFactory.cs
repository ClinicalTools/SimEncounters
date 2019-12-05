using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;
using UnityEngine;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalIconFactory : IconFactory
    {
        protected static NodeInfo LegacyColorFinder { get; } = new NodeInfo("iconColor");
        protected override Color GetColor(XmlDeserializer deserializer)
        {
            var color = base.GetColor(deserializer);
            if (color != Color.clear)
                return color;

            return deserializer.GetColor(LegacyColorFinder);
        }
    }
}