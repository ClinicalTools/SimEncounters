using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public class ImageDataFactory : ISerializationFactory<EncounterImageContent>
    {
        protected virtual ISerializationFactory<Sprite> SpriteFactory { get; } 
        public ImageDataFactory(ISerializationFactory<Sprite> spriteFactory)
        {
            SpriteFactory = spriteFactory;
        }

        protected virtual CollectionInfo IconsInfo { get; } = new CollectionInfo("icons", "icon");
        protected virtual CollectionInfo SpritesInfo { get; } = new CollectionInfo("sprites", "sprite");

        public virtual bool ShouldSerialize(EncounterImageContent value) => value != null;

        public void Serialize(XmlSerializer serializer, EncounterImageContent value)
        {
            //serializer.AddKeyValuePairs(IconsInfo, value.Icons, SpriteFactory);
            serializer.AddKeyValuePairs(SpritesInfo, value.Sprites, SpriteFactory);
        }

        public virtual EncounterImageContent Deserialize(XmlDeserializer deserializer)
        {
            var imageData = CreateImageData(deserializer);

            AddSprites(deserializer, imageData);

            return imageData;
        }
        
        protected virtual EncounterImageContent CreateImageData(XmlDeserializer deserializer) => new EncounterImageContent();

        protected virtual List<KeyValuePair<string, Sprite>> GetSprites(XmlDeserializer deserializer)
            => deserializer.GetKeyValuePairs(SpritesInfo, SpriteFactory);
        protected virtual void AddSprites(XmlDeserializer deserializer, EncounterImageContent imageData)
        {
            var spritePairs = GetSprites(deserializer);
            if (spritePairs == null)
                return;

            foreach (var spritePair in spritePairs)
                imageData.Sprites.Add(spritePair);
        }
    }
}
