using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class ImageDataFactory : ISerializationFactory<EncounterImageData>
    {
        protected virtual ISerializationFactory<LegacyIcon>  IconFactory { get; }
        protected virtual ISerializationFactory<Sprite> SpriteFactory { get; } 
        public ImageDataFactory(ISerializationFactory<LegacyIcon> iconFactory, ISerializationFactory<Sprite> spriteFactory)
        {
            IconFactory = iconFactory;
            SpriteFactory = spriteFactory;
        }

        protected virtual CollectionInfo IconsInfo { get; } = new CollectionInfo("icons", "icon");
        protected virtual CollectionInfo SpritesInfo { get; } = new CollectionInfo("sprites", "sprite");

        public virtual bool ShouldSerialize(EncounterImageData value) => value != null;

        public void Serialize(XmlSerializer serializer, EncounterImageData value)
        {
            serializer.AddKeyValuePairs(IconsInfo, value.Icons, SpriteFactory);
            serializer.AddKeyValuePairs(SpritesInfo, value.Sprites, SpriteFactory);
        }

        public virtual EncounterImageData Deserialize(XmlDeserializer deserializer)
        {
            var imageData = CreateImageData(deserializer);

            AddSprites(deserializer, imageData);

            return imageData;
        }
        
        protected virtual EncounterImageData CreateImageData(XmlDeserializer deserializer) => new EncounterImageData();

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
