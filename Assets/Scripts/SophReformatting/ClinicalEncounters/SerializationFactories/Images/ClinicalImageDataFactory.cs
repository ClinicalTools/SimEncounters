using System.Collections.Generic;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;
using UnityEngine;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalImageDataFactory : ImageDataFactory
    {
        public ClinicalImageDataFactory(ISerializationFactory<Icon> iconFactory, ISerializationFactory<Sprite> spriteFactory)
            : base(iconFactory, spriteFactory) { }

        protected virtual CollectionInfo LegacyImageInfo { get; } =
            new CollectionInfo(
                NodeInfo.RootValue,
                new NodeInfo("image", TagComparison.NameStartsWith)
            );
        
        protected virtual KeyValuePair<NodeInfo, NodeInfo> LegacyImageKeyValueInfo { get; } =
            new KeyValuePair<NodeInfo, NodeInfo>(
                new NodeInfo("key"), 
                new NodeInfo("imgData")
            );

        protected override List<KeyValuePair<string, Icon>> GetIcons(XmlDeserializer deserializer)
        {
            var icons = base.GetIcons(deserializer);
            if (icons != null && icons.Count > 0)
                return icons;
            else
                return deserializer.GetKeyValuePairs(LegacyImageInfo, LegacyImageKeyValueInfo, IconFactory);
        }
        protected override List<KeyValuePair<string, Sprite>> GetSprites(XmlDeserializer deserializer)
        {
            var Sprites = base.GetSprites(deserializer);
            if (Sprites != null && Sprites.Count > 0)
                return Sprites;
            else
                return deserializer.GetKeyValuePairs(LegacyImageInfo, LegacyImageKeyValueInfo, SpriteFactory);
        }
    }
}