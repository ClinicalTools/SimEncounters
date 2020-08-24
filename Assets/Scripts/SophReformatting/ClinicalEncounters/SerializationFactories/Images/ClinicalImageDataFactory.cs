using System.Collections.Generic;
using ClinicalTools.ClinicalEncounters.Data;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;
using UnityEngine;

namespace ClinicalTools.ClinicalEncounters.SerializationFactories
{
    public class ClinicalImageDataFactory : ImageDataFactory
    {
        protected ISerializationFactory<LegacyIcon> IconFactory { get; }
        public ClinicalImageDataFactory(ISerializationFactory<LegacyIcon> iconFactory, ISerializationFactory<Sprite> spriteFactory)
            : base(spriteFactory) {
            IconFactory = iconFactory;
        }

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


        public override EncounterImageContent Deserialize(XmlDeserializer deserializer)
        {
            var imageData = new CEEncounterImageData();

            AddSprites(deserializer, imageData);
            AddIcons(deserializer, imageData);

            return imageData;
        }

        protected virtual void AddIcons(XmlDeserializer deserializer, CEEncounterImageData imageData)
        {
            var iconPairs = GetIcons(deserializer);
            if (iconPairs == null)
                return;

            foreach (var iconPair in iconPairs)
                imageData.LegacyIconsInfo.Add(iconPair.Key, iconPair.Value);
        }
        protected virtual List<KeyValuePair<string, LegacyIcon>> GetIcons(XmlDeserializer deserializer)
        {
            var icons = deserializer.GetKeyValuePairs(IconsInfo, IconFactory);
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