using ClinicalTools.SimEncounters.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Data
{
    public class ImagesData
    {
        public virtual KeyedCollection<Sprite> Sprites { get; } = new KeyedCollection<Sprite>();
        public virtual KeyedCollection<Sprite> Icons { get; } = new KeyedCollection<Sprite>();
        public virtual Dictionary<string, Icon> LegacyIconsInfo { get; } = new Dictionary<string, Icon>();

        protected virtual string DefaultIconsFolder { get; } = "Section Icons";

        public ImagesData()
        {
            var iconSprites = Resources.LoadAll<Sprite>(DefaultIconsFolder);
            foreach (var iconSprite in iconSprites) 
                Icons.Add(iconSprite.name, iconSprite);
        }
    }
}