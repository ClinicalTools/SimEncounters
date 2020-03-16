using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class ImageDataFactory : ISerializationFactory<EncounterImageData>
    {
        protected virtual IconFactory IconFactory { get; } = new IconFactory();
        protected virtual SpriteFactory SpriteFactory { get; } = new SpriteFactory();

        protected virtual CollectionInfo IconsInfo { get; } = new CollectionInfo("icons", "icon");
        protected virtual CollectionInfo SpritesInfo { get; } = new CollectionInfo("sprites", "sprite");

        public virtual bool ShouldSerialize(EncounterImageData value) => value != null;

        public void Serialize(XmlSerializer serializer, EncounterImageData value)
        {
            serializer.AddKeyValuePairs(IconsInfo, value.LegacyIconsInfo, IconFactory);
            serializer.AddKeyValuePairs(SpritesInfo, value.Sprites, SpriteFactory);
        }

        public EncounterImageData Deserialize(XmlDeserializer deserializer)
        {
            var imageData = CreateImageData(deserializer);

            AddIcons(deserializer, imageData);
            AddSprites(deserializer, imageData);

            return imageData;
        }
        
        protected virtual EncounterImageData CreateImageData(XmlDeserializer deserializer) => new EncounterImageData();


        protected virtual List<KeyValuePair<string, Icon>> GetIcons(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(IconsInfo, IconFactory);
        protected virtual void AddIcons(XmlDeserializer deserializer, EncounterImageData imageData)
        {
            var iconPairs = GetIcons(deserializer);
            if (iconPairs == null)
                return;

            foreach (var iconPair in iconPairs)
                imageData.LegacyIconsInfo.Add(iconPair.Key, iconPair.Value);
        }


        protected virtual List<KeyValuePair<string, Sprite>> GetSprites(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(SpritesInfo, SpriteFactory);
        protected virtual void AddSprites(XmlDeserializer deserializer, EncounterImageData imageData)
        {
            var spritePairs = GetSprites(deserializer);
            if (spritePairs == null)
                return;

            foreach (var spritePair in spritePairs)
                imageData.Sprites.Add(spritePair);
        }
    }
}
