using UnityEngine;

namespace ClinicalTools.SimEncounters.SerializationFactories
{
    public interface ISpriteDeserializer
    {
        Sprite Deserialize(int width, int height, string imageData);
    }
}