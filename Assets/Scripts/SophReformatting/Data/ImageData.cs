using UnityEngine;

namespace SimEncounters.Data
{
    public class ImageData
    {
        public virtual KeyedCollection<Sprite> Sprites { get; } = new KeyedCollection<Sprite>();
        public virtual KeyedCollection<Icon> Icons { get; } = new KeyedCollection<Icon>();

        public ImageData() { }
        public ImageData(KeyedCollection<Sprite> sprites, KeyedCollection<Icon> icons) {
            Sprites = sprites;
            Icons = icons;
        }
    }
}