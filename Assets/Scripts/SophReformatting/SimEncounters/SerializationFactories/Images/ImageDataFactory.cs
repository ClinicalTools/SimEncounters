using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class ImageDataFactory : ISerializationFactory<ImageData>
    {
        protected virtual IconFactory IconFactory { get; } = new IconFactory();
        protected virtual SpriteFactory SpriteFactory { get; } = new SpriteFactory();

        protected virtual CollectionInfo IconsInfo { get; } = new CollectionInfo("icons", "icon");
        protected virtual CollectionInfo SpritesInfo { get; } = new CollectionInfo("sprites", "sprite");

        public virtual bool ShouldSerialize(ImageData value) => value != null;

        public void Serialize(XmlSerializer serializer, ImageData value)
        {
            serializer.AddKeyValuePairs(IconsInfo, value.Icons, IconFactory);
            serializer.AddKeyValuePairs(SpritesInfo, value.Sprites, SpriteFactory);
        }

        public ImageData Deserialize(XmlDeserializer deserializer)
        {
            var imageData = CreateImageData(deserializer);

            AddIcons(deserializer, imageData);
            AddSprites(deserializer, imageData);

            return imageData;
        }
        
        protected virtual ImageData CreateImageData(XmlDeserializer deserializer) => new ImageData();


        protected virtual List<KeyValuePair<string, Icon>> GetIcons(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(IconsInfo, IconFactory);
        protected virtual void AddIcons(XmlDeserializer deserializer, ImageData imageData)
        {
            var iconPairs = GetIcons(deserializer);
            if (iconPairs == null)
                return;

            foreach (var iconPair in iconPairs)
                imageData.Icons.Add(iconPair);
        }


        protected virtual List<KeyValuePair<string, Sprite>> GetSprites(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(SpritesInfo, SpriteFactory);
        protected virtual void AddSprites(XmlDeserializer deserializer, ImageData imageData)
        {
            var spritePairs = GetSprites(deserializer);
            if (spritePairs == null)
                return;

            foreach (var spritePair in spritePairs)
                imageData.Sprites.Add(spritePair);
        }
    }
}
